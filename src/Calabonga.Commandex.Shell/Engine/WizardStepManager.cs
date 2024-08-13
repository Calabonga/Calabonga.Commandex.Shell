﻿using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Wizards;
using Calabonga.Commandex.Engine.Wizards.ManagerEventArgs;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (WizardStepManager 2024-08-13 08:01)
/// </summary>
public class WizardStepManager : IWizardStepManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IWizardStep<IWizardStepView, IWizardStepViewModel>> _internalSteps;
    public event EventHandler<ManagerStepActivatedArgs>? ManagerStepActivated;

    public WizardStepManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _internalSteps = _serviceProvider.GetServices<IWizardStep<IWizardStepView, IWizardStepViewModel>>().ToList();
    }

    public void ActivateStep<TPayload>(WizardContext<TPayload> wizardContext)
    {

        if (IsCanDeactivatePreviousStep())
        {
            return;
        }

        _internalSteps.DeactivateAll();

        InitializeStepByIndex(wizardContext);

        var steps = new ObservableCollection<IWizardStep>(_internalSteps);
        var active = _internalSteps.Find(x => x.IsActive);
        OnManagerStepActivated(new ManagerStepActivatedArgs(steps, active));
    }

    public ObservableCollection<IWizardStep> GetSteps() => new(_internalSteps);

    private bool IsCanDeactivatePreviousStep()
    {
        var active = _internalSteps.Find(x => x.IsActive);
        if (active is null)
        {
            return false;
        }

        var activeView = (IWizardStepView)active.Content;
        var activeViewModel = (IWizardStepViewModel)activeView.DataContext!;
        var canLeave = activeViewModel.HasErrors;
        if (canLeave)
        {
            return true;
        }

        activeViewModel.HasErrorsChanged -= OnHasErrorsChanged;

        return false;
    }

    private void OnHasErrorsChanged(object? sender, bool e)
    {
        _internalSteps.Find(x => x.IsActive)?.UpdateHasErrors(e);
    }

    private object? ResolveType(Type type) => _serviceProvider.GetService(type);

    private void InitializeStepByIndex<TPayload>(WizardContext<TPayload> wizardContext)
    {
        var step = _internalSteps[wizardContext.StepIndex];
        var types = step.GetStepTypes();
        var viewType = ResolveType(types[0]);
        var viewModelType = ResolveType(types[1]);
        if (viewType is not IWizardStepView view)
        {
            throw new WizardInvalidOperationException($"Unable to get View object from {nameof(IWizardStepView)}");
        }

        if (viewModelType is not IWizardStepViewModel viewModel)
        {
            throw new WizardInvalidOperationException($"Unable to get View object from {nameof(IWizardStepViewModel)}");
        }

        viewModel.HasErrorsChanged += OnHasErrorsChanged;
        view.DataContext = viewModel;
        step.HasErrors = viewModel.HasErrors;
        step.Activate(view);
    }

    protected virtual void OnManagerStepActivated(ManagerStepActivatedArgs e) => ManagerStepActivated?.Invoke(this, e);
}