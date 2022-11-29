using SpeedCombobox.Html.Adapter;
using System.Collections.Generic;
using System.Linq;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Automation.AutomationInstructions.TestActions.Associations;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters.Controllers;
using Tricentis.Automation.Engines.Representations.Attributes;
using Tricentis.Automation.Engines.Technicals;
using Tricentis.Automation.Engines.Technicals.Html;

namespace SpeedCombobox.Html.Controller
{
    [SupportedAdapter(typeof(SpanComboboxAdapter))]
    public class SpanComboboxAdapterController : ListAdapterController<SpanComboboxAdapter>
    {
        public SpanComboboxAdapterController(SpanComboboxAdapter contextAdapter, ISearchQuery query, Validator validator) : base(contextAdapter, query, validator)
        {
        }

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
        protected override IEnumerable<IAssociation> ResolveAssociation(ListItemsBusinessAssociation businessAssociation)
        {
            yield return new AlgorithmicAssociation("ListItems");
        }
        protected override IEnumerable<ITechnical> SearchTechnicals(IAlgorithmicAssociation algorithmicAssociation)
        {
            if (algorithmicAssociation.AlgorithmName != "ListItems")
            {
                return base.SearchTechnicals(algorithmicAssociation);
            }

            ContextAdapter.OpenComboBox();
            return GetItems();
        }

        private IEnumerable<ITechnical> GetItems()
        {
            IHtmlDocumentTechnical documentTechnical = ContextAdapter.Technical.Document.Get<IHtmlDocumentTechnical>();
            IEnumerable<IHtmlElementTechnical> liItems = documentTechnical.GetElementsByClassName("ui-menu-item").Get<IHtmlElementTechnical>();

            if (liItems == null) return new List<ITechnical>();

            return liItems;
        }
    }
}
