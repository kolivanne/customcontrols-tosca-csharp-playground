using System.Collections.Generic;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Automation.AutomationInstructions.TestActions.Associations;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters.Controllers;
using Tricentis.Automation.Engines.Representations.Attributes;


namespace ZootopiaHtmlTree.Html.Adapter.Controller
{
    [SupportedAdapter(typeof(TreeAdapter))]
    public class TreeAdapterController : TreeContextAdapterController<TreeAdapter>
    {
        public TreeAdapterController(TreeAdapter contextAdapter, ISearchQuery query, Validator validator) : base(contextAdapter, query, validator)
        {
        }

        protected override IEnumerable<IAssociation> ResolveAssociation(ChildrenBusinessAssociation businessAssociation)
        {
            yield return new TechnicalAssociation("Children");
        }

        protected override IEnumerable<IAssociation> ResolveAssociation(DescendantsBusinessAssociation businessAssociation)
        {
            yield return new TechnicalAssociation("All");
        }

        protected override IEnumerable<IAssociation> ResolveAssociation(ParentBusinessAssociation businessAssociation)
        {
            yield return new TechnicalAssociation("ParentNode");
        }

        // direct children of the <ul> element
        protected override IEnumerable<IAssociation> ResolveAssociation(TreeNodeBusinessAssociation businessAssociation)
        {
            yield return new TechnicalAssociation("Children");
        }
    }
}
