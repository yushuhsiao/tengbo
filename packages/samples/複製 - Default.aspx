<%@ Page Language="C#" AutoEventWireup="true" CodeFile="複製 - Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="Scripts/json2.js"></script>
    <script type="text/javascript" src="Scripts/util.js"></script>
    <script type="text/javascript" src="Scripts/superfish/js/superfish.js"></script>
    <%--<link href="Scripts/superfish/css/superfish.css" rel="stylesheet" />--%>
    <%--<link href="Scripts/superfish/css/superfish-navbar.css" rel="stylesheet" />--%>
    <style type="text/css">
        html body { margin: 0px; padding: 0px; width: 100%; height: 100%; font-family: Arial, Helvetica, sans-serif; overflow: hidden; }
        .left { float: left; }
        .right { float: right; }
        .space { width: 1px; height: 1px; }
        .border-top, .border-bottom, .border-left, .border-right {
            background-image: url('images/main_61.gif');
            background-repeat: repeat;
        }

        #main { position:absolute; top:50px; left:0; width:100%; }
        #nav { position:absolute; top:0; left:0; width:100%; height:50px; }
        #nav, #nav * { list-style-type:none; white-space:nowrap; }

        .nav-logo { position:absolute; left:0; top:0; }
        .nav-info { position:absolute; right:0; top:0; }
        .nav-menu { position: absolute; top: 0; left: 200px; font-size: 15px; }
        .nav-menu ul { padding: 0; margin: 0; }
        /*li.nav-menu li { border: thin solid red; }*/
        /*li.nav-menu li:hover { background-color: yellow; }*/

        .menu li { position: relative; }
        .menu ul { position: absolute; display:none; top: 100%; left: 0; }
        .menu > li { float: left; }
        .menu li:hover > ul, .menu li.sfHover > ul { display: block; }
        .menu ul ul { top: 0; left: 100%; }

        .subnav > li { position: static; }
        .subnav > li > ul { width: 100%; }
        .subnav > li > ul > li { float: left; }
        .subnav > li > ul > li > ul { top: 100%; left: 0; }
    </style>
    <script type="text/javascript">
        function logout() { $.api.sExecute({ AdminLogout: {} }, function (obj) { if (obj.LoginResult.t1 == 1) window.location.reload(); }); }

        var doc = {
            root: null, main: null, nav: null, footer: null,
            logout: function () { $.api.sExecute({ AdminLogout: {} }, function (obj) { if (obj.LoginResult.t1 == 1) window.location.reload(); }); }
        }

        $(document).ready(function () {
            doc = { root: $(document), main: $('#main'), nav: $('#nav'), footer: $('.frame-footer') }
            $(window).resize(function () { doc.main.css({ height: doc.root.height() - doc.nav.height() + 'px' }); }).resize();
            //$('#menu').superfish({
            //    pathClass: 'current1',
            //    pathLevels:1
            //});
            //$('.nav-setting').superfish({ useClick: true });
        });
    </script>
</head>
<body>
    <div id="main">
        <iframe src="xxx.aspx" frameborder="0" width="100%" height="100%"></iframe>
    </div>
    <div id="nav">
        <ul>
            <li class="nav-logo">logo</li>
            <li class="nav-menu">
                <ul class="menu subnav">
                    <li>aaa
                        <ul>
                            <li>aaa 1
                                <ul>
                                    <li>1-1
                                        <ul>
                                            <li>1-1-1
                                                <ul>
                                                    <li>1-1-1-1
                                                        <ul>
                                                            <li>1-1-1-1-1</li>
                                                            <li>1-1-1-1-2</li>
                                                            <li>1-1-1-1-3</li>
                                                            <li>1-1-1-1-4</li>
                                                            <li>1-1-1-1-5</li>
                                                        </ul>
                                                    </li>
                                                    <li>1-1-1-2
                                                        <ul>
                                                            <li>1-1-1-1-1</li>
                                                            <li>1-1-1-1-2</li>
                                                            <li>1-1-1-1-3</li>
                                                            <li>1-1-1-1-4</li>
                                                            <li>1-1-1-1-5</li>
                                                        </ul>

                                                    </li>
                                                    <li>1-1-1-3
                                                        <ul>
                                                            <li>1-1-1-1-1</li>
                                                            <li>1-1-1-1-2</li>
                                                            <li>1-1-1-1-3</li>
                                                            <li>1-1-1-1-4</li>
                                                            <li>1-1-1-1-5</li>
                                                        </ul>

                                                    </li>
                                                    <li>1-1-1-4</li>
                                                    <li>1-1-1-5</li>
                                                </ul>
                                            </li>
                                            <li>1-1-2
                                                <ul>
                                                    <li>1-1-2-1
                                                        <ul>
                                                            <li>1-1-1-1-1</li>
                                                            <li>1-1-1-1-2</li>
                                                            <li>1-1-1-1-3</li>
                                                            <li>1-1-1-1-4</li>
                                                            <li>1-1-1-1-5</li>
                                                        </ul>

                                                    </li>
                                                    <li>1-1-2-2
                                                        <ul>
                                                            <li>1-1-1-1-1</li>
                                                            <li>1-1-1-1-2</li>
                                                            <li>1-1-1-1-3</li>
                                                            <li>1-1-1-1-4</li>
                                                            <li>1-1-1-1-5</li>
                                                        </ul>

                                                    </li>
                                                    <li>1-1-2-3
                                                        <ul>
                                                            <li>1-1-1-1-1</li>
                                                            <li>1-1-1-1-2</li>
                                                            <li>1-1-1-1-3</li>
                                                            <li>1-1-1-1-4</li>
                                                            <li>1-1-1-1-5</li>
                                                        </ul>

                                                    </li>
                                                    <li>1-1-2-4</li>
                                                    <li>1-1-2-5</li>
                                                </ul>
                                            </li>
                                            <li>1-1-3
                                                <ul>
                                                    <li>1-1-2-1</li>
                                                    <li>1-1-2-2</li>
                                                    <li>1-1-2-3</li>
                                                    <li>1-1-2-4</li>
                                                    <li>1-1-2-5</li>
                                                </ul>
                                            </li>
                                            <li>1-1-4</li>
                                            <li>1-1-5</li>
                                        </ul>
                                    </li>
                                    <li>1-2
                                        <ul>
                                            <li>1-2-1</li>
                                            <li>1-2-2</li>
                                            <li>1-2-3</li>
                                            <li>1-2-4</li>
                                            <li>1-2-5</li>
                                        </ul>
                                    </li>
                                    <li>1-3
                                        <ul>
                                            <li>1-3-1</li>
                                            <li>1-3-2</li>
                                            <li>1-3-3</li>
                                            <li>1-3-4</li>
                                            <li>1-3-5</li>
                                        </ul>
                                    </li>
                                    <li>1-4</li>
                                    <li>1-5</li>
                                </ul>
                            </li>
                            <li>aaa 2
                                <ul>
                                    <li>2-1</li>
                                    <li>2-2</li>
                                    <li>2-3</li>
                                    <li>2-4</li>
                                    <li>2-5</li>
                                </ul>
                            </li>
                            <li>aaa 3
                                <ul>
                                    <li>3-1</li>
                                    <li>3-2</li>
                                    <li>3-3</li>
                                    <li>3-4</li>
                                    <li>3-5</li>
                                </ul>
                            </li>
                            <li>aaa 4</li>
                            <li>aaa 5</li>
                        </ul>
                    </li>
                    <li>bbb
                        <ul>
                            <li>bbb 1</li>
                            <li>bbb 2</li>
                            <li>bbb 3</li>
                            <li>bbb 4</li>
                            <li>bbb 5</li>
                        </ul>
                    </li>
                    <li>ccc
                        <ul>
                            <li>bbb 1</li>
                            <li>bbb 2</li>
                            <li>bbb 3</li>
                            <li>bbb 4</li>
                            <li>bbb 5</li>
                        </ul>
                    </li>
                    <li>ddd
                        <ul>
                            <li>bbb 1</li>
                            <li>bbb 2</li>
                            <li>bbb 3</li>
                            <li>bbb 4</li>
                            <li>bbb 5</li>
                        </ul>
                    </li>
                    <li>eee
                        <ul>
                            <li>bbb 1</li>
                            <li>bbb 2</li>
                            <li>bbb 3</li>
                            <li>bbb 4</li>
                            <li>bbb 5</li>
                        </ul>
                    </li>
                    <li>xxxxxxxxxxxxx</li>
                </ul>
            </li>
            <li class="nav-info">
                <ul class="menu">
                    <li>admin</li>
                    <li>settings
                        <ul style="left:auto;right:0;">
                            <li>edit</li>
                            <li>logout</li>
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
</body>
</html>
