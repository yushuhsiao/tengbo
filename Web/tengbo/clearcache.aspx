<%@ Page Language="C#" AutoEventWireup="true" Inherits="SitePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link href="css/com.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery.js"></script>

    <style type="text/css">
        .table1 {
            color: #fff;
            font-size: 14px;
            margin-top: 40px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div class="float" id="main">
        <div class="cqk_ck">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center">
                        <table class="table1" border="0" align="center">
                            <tr>
                                <td colspan="3" style="text-align: center;padding-top: 10px;">请同时按
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;padding-top: 10px;">
                                    <label style="font-size: 20px; color: yellow;">CRTL+SHIFT+DELETE</label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;padding-top: 10px;">然后点击“清除浏览数据” 来清理您电脑上的缓存
                                </td>
                            </tr>
                             <tr>
                                <td colspan="3" style="text-align: center; padding-top: 20px;">
                                    <span onclick="window.parent.$.unblockUI();" style="cursor: pointer;">
                                        <a>
                                            <img src="image/quxiao2.gif" width="80" height="34" />
                                        </a>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
</html>
