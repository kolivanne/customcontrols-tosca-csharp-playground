using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Technicals.Html;


namespace ZootopiaHtmlTree.Html.Adapter
{
    [SupportedTechnical(typeof(IHtmlElementTechnical))]
    public class TreeAdapter : AbstractHtmlDomNodeAdapter<IHtmlElementTechnical>, ITreeAdapter
    {
        public TreeAdapter(IHtmlElementTechnical technical, Validator validator) : base(technical, validator)
        {
            validator.AssertTrue(() => technical.Tag.ToLower().Equals("ul") && technical.ClassName.Contains("ui-menu"));
        }

        public override string DefaultName => "Latin Zootopia Tree";
    }
}
