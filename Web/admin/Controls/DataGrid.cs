using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace web.Controls
{
    //partial class DataGrid
    //{
    //    public const string _DataGrid = "DataGrid";

    //    protected override void OnInit(EventArgs e)
    //    {
    //        if (this.Columns != null)
    //        {
    //            ColumnsContainer container = new ColumnsContainer();
    //            this.Columns.InstantiateIn(container);
    //            this.ColumnsPlaceHolder.Controls.Add(container);
    //        }
    //        base.OnInit(e);
    //    }

    //    protected override void OnLoad(EventArgs e)
    //    {
    //        base.OnLoad(e);
    //        List<DataGridColumn> columns = new List<DataGridColumn>(this.GetColumns());
    //    }

    //    public IEnumerable<DataGridColumn> GetColumns()
    //    {
    //        if (this.ColumnsPlaceHolder.Controls.Count > 0)
    //        {
    //            foreach (Control c1 in this.ColumnsPlaceHolder.Controls[0].Controls)
    //            {
    //                DataGridColumn c2 = c1 as DataGridColumn;
    //                if (c2 != null)
    //                    yield return c2;
    //            }
    //        }
    //    }

    //    [TemplateContainer(typeof(ColumnsContainer))]
    //    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    //    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    //    [TemplateInstance(TemplateInstance.Single)]
    //    public ITemplate Columns { get; set; }

    //    public class ColumnsContainer : Control, INamingContainer { }
    //}
    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //partial class DataGridColumn
    //{
    //    [JsonProperty, Category(DataGrid._DataGrid)]
    //    public string Name { get; set; }
    //    [Category(DataGrid._DataGrid)]
    //    public string Text { get; set; }
    //    [Category(DataGrid._DataGrid)]
    //    public string Text_id { get; set; }

    //    [JsonProperty("Text"), Category(DataGrid._DataGrid)]
    //    string _Text
    //    {
    //        get
    //        {
    //            string text = lang[this.Text_id];
    //            if (string.IsNullOrEmpty(text))
    //                text = this.Text ?? this.Name;
    //            return text;
    //        }
    //    }
        
    //    public void aaa() { }

    //    System.Web.UI.WebControls.DataGrid g;
    //}

    [ToolboxData("<{0}:DataGrid runat=server></{0}:DataGrid>")]
    [ParseChildren(true)]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class DataGrid : TemplateControl, INamingContainer
    {
        internal const string TAG = "DataGrid";
        //[PersistenceMode(PersistenceMode.InnerProperty)]
        //[TemplateContainer(typeof(DataGrid))]
        //[TemplateInstance(TemplateInstance.Single)]

        internal Control _Columns = new Control();
        public HtmlGenericControl _Toolbar = new HtmlGenericControl();

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate Columns { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate Toolbar { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual Pager Pager { get; set; }

        [Category(DataGrid.TAG)]
        public string Width { get; set; }

        [Category(DataGrid.TAG)]
        public string Height { get; set; }

        [Category(DataGrid.TAG)]
        public bool ShrinkToFit { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Control g = base.LoadControl("~/Controls/DataGrid.ascx");
            base.Controls.Add(g);
            if (this.Toolbar != null)
            {
                this.Toolbar.InstantiateIn(this._Toolbar);
                this.Controls.Add(this._Toolbar);
                this._Toolbar.TagName = "div";
                this._Toolbar.ID = Guid.NewGuid().ToString();
            }
            if (this.Columns != null)
            {
                this.Columns.InstantiateIn(this._Columns);
                this.Controls.Add(this._Columns);
                this._Columns.Visible = false;
            }
        }

        //protected override void OnInit(System.EventArgs e)
        //{
        //    base.OnInit(e);

        //    // Initialize all child controls.
        //    this.CreateChildControls();
        //    this.ChildControlsCreated = true;
        //}

        //protected override void CreateChildControls()
        //{
        //    // Remove any controls
        //    this.Controls.Clear();

        //    // Add all content to a container.
        //    var container = new Control();
        //    this.Columns.InstantiateIn(container);

        //    // Add container to the control collection.
        //    this.Controls.Add(container);
        //}
    }

    partial class _DataGrid : System.Web.UI.UserControl
    {
        protected DataGrid DataGrid
        {
            [DebuggerStepThrough]
            get { return this.Parent as DataGrid; }
        }

        public override string ClientID
        {
            [DebuggerStepThrough]
            get { return this.DataGrid.ClientID; }
        }

        public IEnumerable<object> GetColumns()
        {
            foreach (Control c in this.DataGrid._Columns.Controls)
                if (c is DataGridColumn)
                    yield return (DataGridColumn)c;
        }

        protected string prop(string format, string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return string.Format(format, value);
        }

        protected string prop(string format, bool value)
        {
            if (value) return string.Format(format, value);
            return null;
        }
    }

    public class Pager
    {
        public string a1 { get; set; }
    }

    [ToolboxData("<{0}:DataGridColumn runat=server Name=\"\"></{0}:DataGridColumn>")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class DataGridColumn : Control
    {
        [Category(DataGrid.TAG), JsonProperty]
        public string Name
        {
            get;
            set;
        }

        [Category(DataGrid.TAG), JsonProperty]
        public bool? IsKey
        {
            get;
            set;
        }

        [Category(DataGrid.TAG), JsonProperty("width")]
        public Unit? Width
        {
            get;
            set;
        }

        [Category(DataGrid.TAG)]
        public string Text
        {
            get;
            set;
        }

        [Category(DataGrid.TAG)]
        public string Text_id
        {
            get;
            set;
        }

        [JsonProperty("Text")]
        string _Text
        {
            get
            {
                string text = Lang.GetLang(this.Page as IPageLang, this.Text_id, 0);
                if (string.IsNullOrEmpty(text)) text = this.Text;
                //if (string.IsNullOrEmpty(text)) text = this.Name;
                return text;
            }
        }

        [JsonProperty]
        public bool? Forzen
        {
            get;
            set;
        }

        [Category(DataGrid.TAG), JsonProperty, PersistenceMode(PersistenceMode.InnerProperty)]
        public _Header Header
        {
            get;
            set;
        }
        public class _Header
        {
        }

        [Category(DataGrid.TAG), JsonProperty, PersistenceMode(PersistenceMode.InnerProperty)]
        public _Viewer Viewer
        {
            get;
            set;
        }
        public class _Viewer
        {
        }

        [Category(DataGrid.TAG), JsonProperty, PersistenceMode(PersistenceMode.InnerProperty)]
        public _Editor Editor { get; set; }
        public class _Editor
        {
        }

        [Category(DataGrid.TAG), JsonProperty, PersistenceMode(PersistenceMode.InnerProperty)]
        public _Filter Filter { get; set; }
        public class _Filter
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write(string.Format(@"<div id=""{0}"">{0}</div>", this.ClientID));
        }
    }

    //public class DataGridColumnDesigner : System.Web.UI.Design.ReadWriteControlDesigner
    //{
    //}
}