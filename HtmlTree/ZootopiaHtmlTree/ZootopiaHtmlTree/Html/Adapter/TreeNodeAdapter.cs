using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Technicals.Html;
using Tricentis.Automation.Simulation;

namespace ZootopiaHtmlTree.Html.Adapter
{
    [SupportedTechnical(typeof(IHtmlElementTechnical))]
    public class TreeNodeAdapter : AbstractHtmlDomNodeAdapter<IHtmlElementTechnical>, IMenuItemAdapter
    {
        public TreeNodeAdapter(IHtmlElementTechnical technical, Validator validator) : base(technical, validator)
        {
            validator.AssertTrue(() => technical.Tag.Equals("li", StringComparison.OrdinalIgnoreCase) && technical.ClassName.Contains("ui-menu-item"));
        }
   
        public override string DefaultName => Name;
        #region Interface IMenuItemAdapter
        public string Name => GetName();


        /// <summary>
        /// Here we check if there is a span element inside the li - if yes, we'll have to use a regular expression 
        /// to retrieve the name - InnerText would not work, since it would also retrieve the names of the subnodes.
        /// </summary>
        private string GetName()
        {
            if (Technical.Children.Get<IHtmlSpanTechnical>().Any())
            {
                Regex name = new Regex(@"span\>(?<title>.*?)\<ul", RegexOptions.Singleline);
                return name.Match(Technical.InnerHtml).Groups["title"].Value.Trim();
            }
            else
            {
                return Technical.InnerText;
            }
        }

        public void Select()
        {
            Mouse.PerformMouseAction(MouseOperation.Click, ActionPoint);
        }
        #endregion
    }
}
