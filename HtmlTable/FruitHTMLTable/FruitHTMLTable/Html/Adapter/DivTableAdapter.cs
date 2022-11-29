using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Technicals.Html;

namespace FruitHTMLTable.Html.Adapter
{
    [SupportedTechnical(typeof(IHtmlDivTechnical))]
    public class DivTableAdapter : AbstractHtmlDomNodeAdapter<IHtmlDivTechnical>, ITableAdapter
    {
        public DivTableAdapter(IHtmlDivTechnical technical, Validator validator) : base(technical, validator)
        {
            validator.AssertTrue(() => !string.IsNullOrEmpty(technical.InnerText) && technical.ClassName.Equals("z-grid z-content"));
        }

        public LoadStrategy LoadStrategy => LoadStrategy.Default;

        public override string DefaultName => "Tutti Frutti Table";
    }
}
