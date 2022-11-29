using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.Automation.Engines.SpecialExecutionTasks;
using Tricentis.Automation;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.SpecialExecutionTasks.Attributes;
using Tricentis.Automation.AutomationInstructions.Configuration;
using Tricentis.Automation.AutomationInstructions.Dynamic.Values;
using Tricentis.Automation.Engines;

namespace BufferSET
{
    [SpecialExecutionTaskName("SetBuffer")]
    public class BufferOperation : SpecialExecutionTaskEnhanced
    {
        public BufferOperation(Validator validator) : base(validator) { }
        public override void ExecuteTask(ISpecialExecutionTaskTestAction testAction)
        {
            //Iterate over each TestStepValue
            foreach (var testStepValue in testAction.Parameters)
            {
                //ActionMode input means set the buffer
                if (testStepValue.ActionMode == ActionMode.Input)
                {
                    IInputValue inputValue = testStepValue.GetAsInputValue();
                    Buffers.Instance.SetBuffer(testStepValue.Name, inputValue.Value, false);
                    testAction.SetResultForParameter(testStepValue, new BufferValueSetPassedActionResult(testStepValue.Name, inputValue.ValueToLog));
                }
                //Otherwise we let TBox handle the verification. Other ActionModes like WaitOn will lead to an exception.
                else
                {
                    if (Buffers.Instance.TryGetBuffer(testStepValue.Name, out string bufferValue))
                    {
                        //Don't need the return value of HandleActualValue in this case.
                        HandleActualValue(testAction, testStepValue, bufferValue);
                    }
                    else
                    {
                        string errorMessage = $"Buffer with name '{testStepValue.Name}' was not found.";
                        testAction.SetResultForParameter(testStepValue, new UnknownFailedActionResult(errorMessage));
                    }
                        
                }
            }  
        }
    }
}
