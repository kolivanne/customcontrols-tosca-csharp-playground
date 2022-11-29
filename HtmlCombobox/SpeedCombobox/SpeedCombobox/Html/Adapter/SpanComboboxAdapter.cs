using System.Linq;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Adapters.Lists;
using Tricentis.Automation.Engines.Technicals.Html;


namespace SpeedCombobox.Html.Adapter
{
    [SupportedTechnical(typeof(IHtmlSpanTechnical))]
    public class SpanComboboxAdapter : AbstractHtmlDomNodeAdapter<IHtmlSpanTechnical>, IComboBoxAdapter
    {
        public SpanComboboxAdapter(IHtmlSpanTechnical technical, Validator validator) : base(technical, validator)
        {
            validator.AssertTrue(() => technical.ClassName.Contains("ui-selectmenu-button"));
        }

        public override string DefaultName => Technical.Id;

        /// <summary>
        /// Open the menu with the dropdown button
        /// </summary>
        public void OpenComboBox()
        {
            string buttonClassName = "ui-icon ui-icon-triangle-1-s";
            IHtmlSpanTechnical boxButton = Technical.Children.Get<IHtmlSpanTechnical>().SingleOrDefault(button => button.ClassName.Equals(buttonClassName));
            if (boxButton != null) boxButton.Click();
        }
    }
}
