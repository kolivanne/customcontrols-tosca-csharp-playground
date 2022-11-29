using System.Collections.Generic;
using System.Linq;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Automation.AutomationInstructions.TestActions.Associations;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters.Controllers;
using Tricentis.Automation.Engines.Representations.Attributes;
using Tricentis.Automation.Engines.Technicals;
using Tricentis.Automation.Engines.Technicals.Html;

namespace ZootopiaHtmlTree.Html.Adapter.Controller
{
    [SupportedAdapter(typeof(TreeNodeAdapter))]
    public class TreeNodeAdapterController : TreeNodeContextAdapterController<TreeNodeAdapter>
    {
        public TreeNodeAdapterController(TreeNodeAdapter contextAdapter, ISearchQuery query, Validator validator) : base(contextAdapter, query, validator)
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
        protected override IEnumerable<IAssociation> ResolveAssociation(TreeNodeBusinessAssociation businessAssociation)
        {
            yield return new AlgorithmicAssociation("SubNodes");
        }
        protected override IEnumerable<ITechnical> SearchTechnicals(IAlgorithmicAssociation algorithmicAssociation)
        {
            if (algorithmicAssociation.AlgorithmName == "SubNodes")
            {
                return GetSubNodes();
            }
            return base.SearchTechnicals(algorithmicAssociation);
        }

        private IEnumerable<ITechnical> GetSubNodes()
        {
            IHtmlElementTechnical ulElement = ContextAdapter.Technical.Children.Get<IHtmlElementTechnical>().FirstOrDefault(t => t.Tag.ToLower() == "ul");
            if (ulElement != null)
            { 
                return ulElement.Children.Get<ITechnical>();
            }
            return new ITechnical[] { };
        }
    }
}
