<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Import Namespace="BU" %>
<%@ Import Namespace="web" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Tools.Protocol" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">

    //CorpList data;

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.data = CorpList.GetCorps.execute(null, null);
    }
    
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var table;

        $(document).ready(function () {
            table = $("#_data").jqGrid({
                url: 'api',
                mtype: 'POST',
                datatype: 'json',
                beforeRequest: function () {
                    console.log('beforeRequest');
                },
                loadBeforeSend: function (xhr, settings) {
                    console.log('loadBeforeSend');
                },
                serializeGridData: function (postData) {
                    return { str: JSON.stringify({ GetCorps: { jqgrid: postData } }) };
                    console.log('serializeGridData');
                },
                loadError: function (xhr, status, error) {
                    console.log('loadError');
                },
                beforeProcessing: function (data, status, xhr) {
                    if (data.CorpList) {
                        var CorpList = data.CorpList;
                        delete data.CorpList;
                        if (CorpList.items) {
                            for (var i = 0; i < CorpList.items.length; i++) {
                                var item = CorpList.items[i];
                                item.at = Date.fromISO(item.at);// $.parseDate(item.at);
                                item.et = Date.fromISO(item.et);
                                item.ct = Date.fromISO(item.ct);
                                item.mt = Date.fromISO(item.mt);
                            }
                            data.rows = CorpList.items;
                        }
                        if (CorpList.jqGrid) {
                            var jqGrid = CorpList.jqGrid;
                            delete CorpList.jqGrid;
                            $.extend(data, jqGrid);
                        }
                    }
                    console.log('beforeProcessing');
                    //table[0].addJSONData(data);
                    //return false;
                    return true;
                },
                gridComplete: function () {
                    console.log('gridComplete');
                },
                loadComplete: function (data) {
                    console.log('loadComplete');
                },
                height: 300,
                colModel: [
                    { label: '<%=GetLocalResourceObject("CorpID")    %>', name: 'id', key: true, index: 'id', width: 60, sorttype: "int" },
                    { label: '<%=GetLocalResourceObject("Domain")    %>', name: 'domain', index: 'domain', width: 90, sorttype: "text" },
                    { label: '<%=GetLocalResourceObject("Name")      %>', name: 'name', index: 'name', width: 100 },
                    { label: '<%=GetLocalResourceObject("ActiveTime")%>', name: 'at', index: 'at', width: 80, sorttype: "date", formatter: "date" },
                    { label: '<%=GetLocalResourceObject("ExpireTime")%>', name: 'et', index: 'et', width: 80, sorttype: "date", formatter: "date" },
                    { label: '<%=GetLocalResourceObject("CreateTime")%>', name: 'ct', index: 'ct', width: 80, sorttype: "date", formatter: "date" },
                    { label: '<%=GetLocalResourceObject("ModifyTime")%>', name: 'mt', index: 'mt', width: 80, sorttype: "date", formatter: "date" },
                    { label: '<%=GetLocalResourceObject("ModifyUser")%>', name: 'muser', index: 'muser', width: 80, sorttype: "string" }
                ],
                rowNum: 10,
                rowList: [10, 20, 30],
                pager: '#_pager',
                sortname: 'id',
                emptyrecords: "Nothing to display",
                viewrecords: true,
                sortorder: "asc",
                autowidth: true,
                //headertitles:true,
                multiselect: false,
                rownumbers: true,
                toolbar: [true, '#_tool'],
                jsonReader: {
                    //root: 'CorpList.items',
                    //page: "CorpList.jqGrid.page",
                    //total: "CorpList.jqGrid.total",
                    //records: "CorpList.jqGrid.records",
                    repeatitems: false
                },
                //data: execute_success(JSON.parse('<1%= api.Serialize(this.data) %>'))
                //footerrow: true,
                //caption: "Manipulating Array Data"
                onSelectRow: function (id) {
                    console.log(id);
                    //if (id && id !== lastsel) {
                    //    jQuery('#rowed3').jqGrid('restoreRow', lastsel);
                    //    jQuery('#rowed3').jqGrid('editRow', id, true);
                    //    lastsel = id;
                    //}
                }
            });

            table.navGrid("#_pager", { search: false, edit: false, add: false, del: false });


   

            //table.navGrid('#_pager', { edit: true, add: true, del: true, view: true });



            //execute_success(JSON.parse('<!%= api.Serialize(this.data) %>'));
            //for (var i = 0; i < mydata.length; i++) {
            //table.addRowData(mydata[i].id, mydata[i]);
            //}
            //$("#list4").jqGrid('addRowData', i + 1, mydata[i]);

            //$('#page_css').attr('href', 'Scripts/jquery-ui/themes/dark-hive/jquery-ui.css')
        });

        //function execute_success(obj) {
        //    if (obj.CorpList) {
        //        if (obj.CorpList.items) {
        //            for (var i = 0; i < obj.CorpList.items.length; i++) {
        //                var item = obj.CorpList.items[i];
        //                item.at = util.parseDate(item.at);
        //                item.et = util.parseDate(item.et);
        //                item.ct = util.parseDate(item.ct);
        //                item.mt = util.parseDate(item.mt);
        //                //table.addRowData(item.id, item, null, item.id);
        //            }
        //            //table.trigger("reloadGrid");
        //            return obj.CorpList.items;
        //        }
        //    }
        //};



        //$.fn.jqGrid2 = function (options) {
        //    var p = {
        //        beforeRequest: function () {
        //        },
        //        loadBeforeSend: function (xhr, settings) {
        //            settings.dataTypes = [settings.dataType = 'xml'];
        //        },
        //        serializeGridData: function (postData) {
        //            if (options._serializeGridData) {
        //                var command = options._serializeGridData(postData);
        //                if (command) {
        //                    for (var attr in command) {
        //                        command[attr]._jqGrid = postData;
        //                        break;
        //                    }
        //                    return { str: JSON.stringify(command) }
        //                }
        //            }
        //        },
        //        loadError: function (xhr, status, error) {
        //        },
        //        beforeProcessing: function (data, status, xhr) {
        //            var txt = $('string', data).text();
        //            var obj = JSON.parse(txt);
        //            obj = JSON.parse(obj);
        //            if (obj.CorpList) {
        //                if (obj.CorpList.items) {
        //                    for (var i = 0; i < obj.CorpList.items.length; i++) {
        //                        var item = obj.CorpList.items[i];
        //                        item.at = util.parseDate(item.at);
        //                        item.et = util.parseDate(item.et);
        //                        item.ct = util.parseDate(item.ct);
        //                        item.mt = util.parseDate(item.mt);
        //                    }
        //                    //table.addRowData(null, obj.CorpList.items);
        //                }
        //            }
        //            //table.addJSONData(data);
        //            return true;
        //            return false;


        //            //try {
        //            //    var txt = $('string', data).text();
        //            //    //data = txt;
        //            //    data = {
        //            //        page: 10,
        //            //        records: 9999,
        //            //        total: 123,
        //            //        rows: {}
        //            //        //rows: JSON.parse(txt).CorpList.items,
        //            //    };
        //            //    var _corplist = JSON.parse(txt).CorpList.items;
        //            //    for (var i = 0; i < _corplist.length; i++) {
        //            //        data.rows[i] = {
        //            //            id: _corplist[i].id,
        //            //            cell: [
        //            //                _corplist[i].id,
        //            //                _corplist[i].domain,
        //            //                _corplist[i].name,
        //            //                _corplist[i].at,
        //            //                _corplist[i].et,
        //            //                _corplist[i].ct,
        //            //                _corplist[i].mt,
        //            //                _corplist[i].muser
        //            //            ]
        //            //        }
        //            //    }
        //            //    table.addJSONData(data);
        //            //    return false;
        //            //} catch (ex) {
        //            //    if (parse_error) {
        //            //        parse_error(data);
        //            //    }
        //            //}
        //        },
        //        gridComplete: function () {
        //            console.log('gridComplete');
        //        },
        //        loadComplete: function (data) {
        //        },
        //        url: util.url,
        //        mtype: 'POST',
        //        datatype: 'json'
        //    };
        //    for (var attr in options)
        //        p[attr] = options[attr];
        //    return this.jqGrid(p);
        //}
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <table id="_data"></table>
    <div id="_tool"></div>
    <div id="_pager"></div>
</asp:Content>

