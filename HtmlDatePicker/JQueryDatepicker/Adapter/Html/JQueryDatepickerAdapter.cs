using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Tricentis.Automation.AutomationInstructions.Configuration;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Adapters;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html.Generic;
using Tricentis.Automation.Engines.Technicals.Html;
using Tricentis.Automation.Execution;

namespace JQueryDatepicker.Adapter.Html {
    // This class represents (or interacts with) the datepicker. The class implements the ITextBoxAdapter, allowing the framework to identify
    // this Adapter as a TextBox, because there is no such representation like a Datepicker yet.

    // Supported Technical describes the type of HTML element that this datepicker is represented by in the actual HTML structure. In this case 
    // the datepicker is represented by an input-tag.
    [SupportedTechnical(typeof(IHtmlInputElementTechnical))]
    public class JQueryDatepickerAdapter : AbstractHtmlDomNodeAdapter<IHtmlInputElementTechnical>, ITextBoxAdapter {

        #region Fields

        private IHtmlDivTechnical popupContainer;

        #endregion

        #region Properties

        // Returns the value from the input element. Selects the date from the datepicker.
        // The Text property is also called at the very beginning with an empty string to clear the Text.
        public string Text {
            get { return Technical.Value; }
            set { SetDate(value); }
        }

        public bool IsPasswordField {
            get { return false; }
        }

        public bool IsMultilineField {
            get { return false; }
        }

        // DefaultName is the property, that shows up in the scan result as the label/logical name of this item.
        public override string DefaultName {
            get { return String.Format("Datepicker: {0}", Technical.Id); }
        }

        // This is the HTML element of the popup container that is shown when the datepicker is opened
        public IHtmlDivTechnical PopupContainer {
            get {
                if (popupContainer == null) {
                    IHtmlDocumentTechnical document = Technical.Document.Get<IHtmlDocumentTechnical>();
                    popupContainer = document.GetById("ui-datepicker-div").Get<IHtmlDivTechnical>();
                }
                return popupContainer;
            }
        }

        #endregion

        #region Constructors & Destructors

        // The class inherits common functionality from the AbstractHtmlDomNodeAdapter, so that we do not have to implement all the 
        // common methods and properties that work the same for all HtmlDomNodes. The AbstactHtmlDomNodeAdapter needs to be parametrized with
        // the technical it interacts with - IHtmlInputElementTechnical in this case (the same as the SupportedTechnical).

        // This class also implements the ITextBoxAdapter, telling the framework that this Adapter works as textbox. This allows the
        // framework to determine that this Adapter works for the according Representation

        // A constructor is necessary since there is no default constructor. The technical that this adapter is responsible for will be
        // passed in by the framework. Additionally there is a validator. This validator should be used to determine if the adapter fits
        // this technical, e.g. if this adapter is for a datepicker that is represented by an input-tag of type 'text' and the framework passes
        // in an input element of type 'checkbox' then the validator should fail and thereby inform the framework that this combination will not work.
        // In other words, the validator can be used to tell if the technical is a datepicker or not.
        public JQueryDatepickerAdapter(IHtmlInputElementTechnical technical, Validator validator)
            : base(technical, validator) {
            validator.AssertTrue(IsDatepicker(technical));
        }

        #endregion

        #region Methods

        // Determines if the HTML element is identified as a datepicker
        private bool IsDatepicker(IHtmlInputElementTechnical technical) {
            string className = technical.ClassName;
            return !String.IsNullOrEmpty(className) && className.Contains("hasDatepicker");
        }

        // This method gets all text parts from the test step, allowing different actions inbetween entering the text.
        // E.g. if a test step has the value 'abc{Click}def', the framework will call AppendText with 'abc', then will perform a
        // click operation and afterwards AppendText will be called with 'def'. In this case the method simply calls the Text property
        // and will replace the Date every time it is called, as long as the text is a valid date.
        public void AppendText(string text) {
            Text = text;
        }

        // The ControlArea defines the rectangle of the datepicker (e.g. shown when the control is marked in the scan).
        // Typically this does not need to be overriden, but in this case the starting input element is not representing the entire
        // datepicker. There is an icon next to the control and the parent element defines the datepicker group.
        public override RectangleF GetControlArea(bool refresh) {
            IHtmlDivTechnical parentTechnical = Technical.ParentNode.Get<IHtmlDivTechnical>();
            IGuiAdapter parentAdapter = AdapterFactory.CreateAdapters<IGuiAdapter>(parentTechnical, "Html").Single();
            return parentAdapter.GetControlArea(refresh);
        }

        // Main method used to select the specified date on the datepicker, for that
        // * the date needs to be parsed with the given format
        // * the datepicker needs to be opened
        // * year, month and day need to be selected on the datepicker
        public void SetDate(string value) {
            if (String.IsNullOrEmpty(value)) {
                return;
            }
            DateTime date = ParseDate(value);
            Open();
            SetYear(date);
            SetMonth(date);
            SetDay(date);
        }

        // In order to set the date the string value needs to be parsed to a valid DateTime object.
        // The expected date format of the string is configured in Tosca.
        private DateTime ParseDate(string value) {
            return DateTime.ParseExact(value, GetToscaDateFormat(), CultureInfo.InvariantCulture);
        }

        // The MainConfiguration singleton provides access to settings and configuration parameters.
        // The Tosca date format is stored in the Tosca settings and will be used by default to interpret a string as a date.
        // It is also possible to override the setting with the test configuration parameter 'ToscaDateFormat' for e.g. a certain test case.
        // This method returns the expected date format from either the configuration parameter if defined or the settings.
        private string GetToscaDateFormat() {
            string format;
            if (!MainConfiguration.Instance.TryGet("ToscaDateFormat", out format)) {
                format = MainConfiguration.Instance.Get("TBox.Dynamic temporal expressions.Tosca Date format");
            }
            return format;
        }

        // To be able to steer the datepicker it needs to be open.
        // This methods opens the datepicker if it's not already shown on the page.
        // If the popup does not exist even after clicking, an AutomationException is thrown to attempt a retry.
        private void Open() {
            if (PopupContainer == null || !PopupContainer.Visible) {
                Technical.Click();
            }
            if (PopupContainer == null) {
                throw new AutomationException("Could not open datepicker!", AutomationException.SynchronizationAction.Synchronize);
            }
        }

        // Searches for the year dropdown and then selects the year
        private void SetYear(DateTime date) {
            IHtmlSelectTechnical yearContainer =
                PopupContainer.GetElementByTagName("select").Get<IHtmlSelectTechnical>()
                    .Single(select => select.ClassName == "ui-datepicker-year");
            SetDropDownValue(yearContainer, date.Year.ToString());
        }

        // Searches for the month dropdown and then selects the month
        // date.Month - 1 is necessary because the dropdown values are 0-based (0 for January, 1 for February etc.)
        private void SetMonth(DateTime date) {
            IHtmlSelectTechnical monthContainer =
                PopupContainer.GetElementByTagName("select").Get<IHtmlSelectTechnical>()
                    .Single(select => select.ClassName == "ui-datepicker-month");
            SetDropDownValue(monthContainer, (date.Month - 1).ToString());
        }


        // This method sets the specified year or month on the passed dropdown (identified by value)
        // If an option element within the selection element matches the searchValue it will be selected
        // Then the option element will be set to 'Selected' and a 'change' event will be fired to inform the parent control
        // about the selection change.
        private void SetDropDownValue(IHtmlSelectTechnical selectTechnical, string searchValue) {
            IHtmlOptionTechnical optionTechnical =
                selectTechnical.Children.Get<IHtmlOptionTechnical>()
                    .SingleOrDefault(o => o.Value.Trim() == searchValue);
            if (optionTechnical == null) {
                throw new AutomationException(
                    String.Format("The value '{0}' was out of range in '{1}'!", searchValue, selectTechnical.ClassName),
                    AutomationException.SynchronizationAction.DoNotSynchronize);
            }
            optionTechnical.Selected = true;
            optionTechnical.FireEvent("change");
        }

        // Selects the day. For that the calendar element is being taken, which contains all the day elements.
        // The day element has an a-tag element as a child, which will be used to perform a click.
        private void SetDay(DateTime date) {
            IHtmlTableTechnical daysContainer =
                PopupContainer.Children.Get<IHtmlTableTechnical>().Single(t => t.ClassName == "ui-datepicker-calendar");
            IHtmlCellTechnical dayContainerTechnical =
                daysContainer.GetElementByTagName("td").Get<IHtmlCellTechnical>().Single(td => IsSearchedDay(td, date.Day));
            IHtmlAnchorTechnical dayTechnical = dayContainerTechnical.Children.Get<IHtmlAnchorTechnical>().Single();
            dayTechnical.Click();
        }

        // Verifies if the td-tag element is a selectable day and if the text matches with the day being searched
        private bool IsSearchedDay(IHtmlCellTechnical cellTechnical, int searchDay) {
            string dataHandler = cellTechnical.GetAttribute("data-handler");
            return !String.IsNullOrEmpty(dataHandler) && dataHandler == "selectDay"
                        && cellTechnical.InnerText.Trim() == searchDay.ToString();
        }

        #endregion
    }
}
