using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Wizards;
using Calabonga.Commandex.Engine.Wizards.ManagerEventArgs;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Calabonga.Commandex.Shell.Engine;

/// <summary>
/// // Calabonga: Summary required (WizardManager 2024-08-13 08:01)
/// </summary>
public class WizardManager<TPayload> : IWizardManager<TPayload>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IWizardStep<IWizardStepView, IWizardStepViewModel<TPayload>>> _internalSteps;

    public WizardManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _internalSteps = _serviceProvider.GetServices<IWizardStep<IWizardStepView, IWizardStepViewModel<TPayload>>>().ToList();
    }

    public void ActivateStep(WizardContext<TPayload> wizardContext)
    {
        if (IsCanDeactivatePreviousStep())
        {
            return;
        }

        _internalSteps.DeactivateAll();

        GetFromContainer(wizardContext);

        var steps = new ObservableCollection<IWizardStep>(_internalSteps);
        var activeStep = _internalSteps.Find(x => x.IsActive);

        WeakReferenceMessenger.Default.Send(new ManagerStepActivatedMessage(steps, activeStep));
    }

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

        return false;
    }

    /// <summary>
    /// // Calabonga: Summary required (WizardManager 2024-08-13 05:16)
    /// </summary>
    /// <param name="wizardContext"></param>
    /// <exception cref="WizardInvalidOperationException"></exception>
    private void GetFromContainer(WizardContext<TPayload> wizardContext)
    {
        var step = _internalSteps[wizardContext.StepIndex];
        var types = step.GetStepTypes();
        var viewType = _serviceProvider.GetService(types[0]);
        var viewModelType = _serviceProvider.GetService(types[1]);
        if (viewType is not IWizardStepView view)
        {
            throw new WizardInvalidOperationException($"Unable to get View object from {nameof(IWizardStepView)}");
        }

        if (viewModelType is not IWizardStepViewModel<TPayload> viewModel)
        {
            throw new WizardInvalidOperationException($"Unable to get View object from {nameof(IWizardStepViewModel)}");
        }

        viewModel.Initialize(wizardContext.Payload);
        view.DataContext = viewModel;
        step.HasErrors = viewModel.HasErrors;
        step.Activate(view);
    }
}