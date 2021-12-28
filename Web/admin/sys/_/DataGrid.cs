using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace web
{
    [ParseChildren(typeof(DataGridColumn))]
    [PersistChildren(true)]
    public class DataGrid : web.usercontrol
    {
        protected override void OnLoad(EventArgs e)
        {
            List<DataGridColumn> columns = new List<DataGridColumn>();
            foreach (Control c in this.Controls)
            {
                DataGridColumn cc = c as DataGridColumn;
                if (cc == null) continue;
                columns.Add(cc);
            }
            base.OnLoad(e);
        }
    }

    public class DataGridColumn : web.usercontrol
    {
        //public override void RenderBeginTag(HtmlTextWriter writer)
        //{
        //    base.RenderBeginTag(writer);
        //}

        //protected override void RenderContents(HtmlTextWriter writer)
        //{
        //    writer.Write(1);
        //    base.RenderContents(writer);
        //}

        //public override void RenderEndTag(HtmlTextWriter writer)
        //{
        //    base.RenderEndTag(writer);
        //}
    }


    //[ParseChildren(typeof(DataGridColumn))]
    //public class DataGrid : Control
    //{
    //    public override ClientIDMode ClientIDMode
    //    {
    //        get { return System.Web.UI.ClientIDMode.Static; }
    //        set { base.ClientIDMode = value; }
    //    }

    //    void xx(){
    //        System.Web.UI.WebControls.MultiView a;
    //        System.Web.UI.WebControls.View b;
    //    }

    //    public override void RenderControl(HtmlTextWriter writer)
    //    {
    //        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
    //        writer.RenderBeginTag(HtmlTextWriterTag.Div);
    //        writer.RenderEndTag();
    //        //writer.WriteBeginTag("div");
    //        //writer.WriteFullBeginTag("div");
    //        //writer.WriteAttribute("id", this.ClientID);
    //        //writer.WriteEndTag("div");
    //        base.RenderControl(writer);
    //    }
    //}
    //public class DataGridColumn : Control
    //{
    //    public override void RenderControl(HtmlTextWriter writer)
    //    {
    //        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
    //        writer.RenderBeginTag(HtmlTextWriterTag.Div);
    //        writer.RenderEndTag();
    //        base.RenderControl(writer);
    //    }
    //}
}