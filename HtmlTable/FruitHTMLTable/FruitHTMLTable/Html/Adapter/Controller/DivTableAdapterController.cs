using System.Collections.Generic;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Automation.AutomationInstructions.TestActions.Associations;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters.Controllers;
using Tricentis.Automation.Engines.Representations.Attributes;
using Tricentis.Automation.Engines.Technicals;
using Tricentis.Automation.Engines.Technicals.Html;
using Tricentis.Automation.Engines.Technicals.References;
using System.Linq;
using System;

namespace FruitHTMLTable.Html.Adapter.Controller
{
    [SupportedAdapter(typeof(DivTableAdapter))]
    public class DivTableAdapterController : TableContextAdapterController<DivTableAdapter>
    {
        public DivTableAdapterController(DivTableAdapter contextAdapter, ISearchQuery query, Validator validator) : base(contextAdapter, query, validator)
        {
        }

        #region Overrides of ContextAdapterController<DivTableAdapter>

        protected override IEnumerable<IAssociation> ResolveAssociation(ChildrenBusinessAssociation businessAssociation)
        {
            yield return new TechnicalAssociation("Children");
        }

        protected override IEnumerable<IAssociation> ResolveAssociation(ParentBusinessAssociation businessAssociation)
        {
            yield return new TechnicalAssociation("ParentNode");
        }
        protected override IEnumerable<IAssociation> ResolveAssociation(DescendantsBusinessAssociation businessAssociation)
        {
            yield return new TechnicalAssociation("All");
        }
        protected override IEnumerable<IAssociation> ResolveAssociation(ColumnsBusinessAssociation businessAssociation)
        {
            throw new NotSupportedException();
        }
        protected override IEnumerable<IAssociation> ResolveAssociation(RowsBusinessAssociation businessAssociation)
        {
            yield return new AlgorithmicAssociation("Rows");
        }
        protected override IEnumerable<ITechnical> SearchTechnicals(IAlgorithmicAssociation algorithmicAssociation)
        {
            return (algorithmicAssociation.AlgorithmName != "Rows") ? base.SearchTechnicals(algorithmicAssociation) : GetRows();
        }

        private IEnumerable<ITechnical> GetRows()
        {
            return ContextAdapter.Technical.ParentNode.Get<IHtmlDivTechnical>()
                       .ParentNode.Get<IHtmlDivTechnical>()
                       .All.Get<ITechnical>();
        }
        #endregion
    }
}
