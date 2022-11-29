using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Adapters.Lists;
using Tricentis.Automation.Engines.Technicals.Html;

namespace SpeedCombobox.Html.Adapter
{
    [SupportedTechnical(typeof(IHtmlElementTechnical))]
    public class UlComboboxItemAdapter : AbstractHtmlDomNodeAdapter<IHtmlElementTechnical>, IListItemAdapter
    {
       

        public UlComboboxItemAdapter(IHtmlElementTechnical technical, Validator validator) : base(technical, validator)
        {
            validator.AssertTrue(() => technical.ClassName.Contains("ui-menu-item"));
        }
       
        public override string DefaultName => Text;
        public override bool IsSteerable => true;

        public bool Selected 
        {
            get => Technical.ClassName.Contains("ui-state-focus");
            set
            {
                if (value)
                {
                    Technical.FireEvent("mouseover");
                    Technical.Click();
                }
            }
        }

        public string Text => Technical.InnerText;
    }
}
