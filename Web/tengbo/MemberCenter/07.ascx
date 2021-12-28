<%@ Control Language="C#" AutoEventWireup="true" Inherits="SiteControl" %>

<div class="user_center_content" index="07" style="display: <%=this.css_display%>;">
    <script type="text/javascript">
    $(document).ready(function () {
        $('#<%=randID("01")%>,#<%=randID("02")%>,#<%=randID("03")%>,#<%=randID("04")%>,#<%=randID("05")%>,#<%=randID("06")%>').trigger('click');
    });

    function <%=randID("f1")%>_($game_bal) {
            var $txt = $('.game_balance', $game_bal);
            if ($txt.hasClass('loading') == true)
                return true;
            $txt.empty().addClass('loading');

            $.invoke_api({ UserGameBalance: { GameID: $game_bal.attr('gameid') } }, {
                success: function (data, textStatus, jqXHR) {
                    if (data.Status == 1)
                        $txt.text(data.row);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $txt.text('N/A');
                },
                complete: function (jqXHR, textStatus) {
                    $txt.removeClass('loading');
                }
            });
        }

        function <%=randID("f2")%>_($this, gameid) {
            var postData = $this.getPostData();
            var $game_bal = $('#' + postData[gameid]);
            postData[gameid] = $game_bal.attr('gameid');
            $('.user_errmsg', $this).text('');
            if (postData[gameid] == undefined) {
                $('.user_errmsg', $this).text('请选择游戏厅');
                return true;
            }
            postData.amount = parseInt(postData.amount);
            if ((postData.amount <= 0) || isNaN(postData.amount)) {
                $('.user_errmsg', $this).text('请输入金额');
                $('input[name="amount"]', $this).val('');
                return true;
            }
            $('.user_center').block({
                message: '<div class="user_center_busy"></div>',
                centerY: false,
                css: { top: 170 },
                overlayCSS: { backgroundColor: '#000', opacity: 0.6 }
            });

            $.invoke_api({ UserGameTran: postData }, {
                success: function (data, textStatus, jqXHR) {
                    //console.log(arguments);
                    if (data.Status == 1) {
                        if (data.row.Balance) {
                            $('.user_balance').text(data.row.Balance);
                        }
                        if (data.row.GameBalance) {
                            $('.game_balance', $game_bal).text(data.row.GameBalance);
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //console.log(arguments);
                },
                complete: function (jqXHR, textStatus) {
                    $('.user_center').unblock()
                    $('input[type="text"]', $this).val('');
                    $('input[type="radio"]', $this).prop('checked', false);
                }
            });
        }
    </script>
    <div class="item_title">户内转账</div>
    <table class="gametran" cellpadding="0" cellspacing="0">
        <tr>
            <td class="title1">主账户额度：</td>
            <td class="user_balance red"><%=this.Member.BalanceString()%></td>
        </tr>
        <tr>
            <td class="title1">游戏厅额度：</td>
            <td>
                <div id='<%=randID("02")%>' gameid="<%=BU.GameID.AG_AGIN  %>" class="game_select ui-corner-all" onclick="<%=randID("f1")%>_($('#<%=randID("02")%>'), '<%=BU.GameID.AG_AGIN  %>')">
                    <div class="game_title">AG国际厅</div>
                    <div class="game_balance ui-corner-all">0.00</div>
                </div>
                <div id="<%=randID("01")%>" gameid="<%=BU.GameID.AG_AG  %>" class="game_select ui-corner-all" onclick="<%=randID("f1")%>_($('#<%=randID("01")%>'), '<%=BU.GameID.AG_AG  %>')">
                    <div class="game_title">AG旗舰厅</div>
                    <div class="game_balance ui-corner-all">0.00</div>
                </div>
                <div id="<%=randID("03")%>" gameid="<%=BU.GameID.AG_DSP  %>" class="game_select ui-corner-all" onclick="<%=randID("f1")%>_($('#<%=randID("03")%>'), '<%=BU.GameID.AG_DSP  %>')">
                    <div class="game_title">AG实地厅</div>
                    <div class="game_balance ui-corner-all">0.00</div>
                </div>
                <p style="height: 15px;">&nbsp;</p>
                <div id='<%=randID("04")%>' gameid="<%=BU.GameID.BBIN%>" class="game_select ui-corner-all" onclick="<%=randID("f1")%>_($('#<%=randID("04")%>'), '<%=BU.GameID.BBIN%>')">
                    <div class="game_title">波音厅  </div>
                    <div class="game_balance ui-corner-all">0.00</div>
                </div>
                <div id="<%=randID("05")%>" gameid="<%=BU.GameID.HG  %>" class="game_select ui-corner-all" onclick="<%=randID("f1")%>_($('#<%=randID("05")%>'), '<%=BU.GameID.HG  %>')">
                    <div class="game_title">HG厅    </div>
                    <div class="game_balance ui-corner-all">0.00</div>
                </div>
                <div id="<%=randID("06")%>" gameid="<%=BU.GameID.PT  %>" class="game_select ui-corner-all" onclick="<%=randID("f1")%>_($('#<%=randID("06")%>'), '<%=BU.GameID.PT  %>')" style="display: none;">
                    <div class="game_title">PT厅    </div>
                    <div class="game_balance ui-corner-all">0.00</div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="color: #b29b9f;">注：查询时，请点击游戏厅刷新额度</td>
        </tr>
    </table>
    <table id="gamedeposit" class="gametran" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="title2">游戏额度从主账户转出至游戏厅</td>
        </tr>
        <tr>
            <td class="title1">转入游戏厅：</td>
            <td>
                <input type="radio" name="gameid_0" id='<%=randID("12")%>' value="<%=randID("02")%>" /><label for="<%=randID("12")%>">AG国际厅</label>
                <input type="radio" name="gameid_0" id="<%=randID("11")%>" value="<%=randID("01")%>" /><label for="<%=randID("11")%>">AG旗舰厅</label>
                <input type="radio" name="gameid_0" id="<%=randID("13")%>" value="<%=randID("03")%>" /><label for="<%=randID("13")%>">AG实地厅</label>
                <input type="radio" name="gameid_0" id="<%=randID("14")%>" value="<%=randID("04")%>" /><label for="<%=randID("14")%>">波音厅</label>
                <input type="radio" name="gameid_0" id="<%=randID("15")%>" value="<%=randID("05")%>" /><label for="<%=randID("15")%>">HG厅</label>
                <input type="radio" name="gameid_0" id="<%=randID("16")%>" value="<%=randID("06")%>" style="display: none;" /><label for="<%=randID("16")%>" style="display: none;">PT厅</label>
            </td>
        </tr>
        <tr>
            <td class="title1">转入额度：</td>
            <td>
                <input type="text" name="amount" /><input type="button" class="info_button" value="提交" onclick="<%=randID("f2")%>_($('#gamedeposit'), 'gameid_0')" /><span class="user_errmsg"></span></td>
        </tr>
    </table>
    <table id="gamewithdrawal" class="gametran" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="title2">游戏额度从游戏厅转入至主账户</td>
        </tr>
        <tr>
            <td class="title1">转出游戏厅：</td>
            <td>
                <input type="radio" name="gameid_1" id='<%=randID("22")%>' value="<%=randID("02")%>" /><label for="<%=randID("22")%>">AG国际厅</label>
                <input type="radio" name="gameid_1" id="<%=randID("21")%>" value="<%=randID("01")%>" /><label for="<%=randID("21")%>">AG旗舰厅</label>
                <input type="radio" name="gameid_1" id="<%=randID("23")%>" value="<%=randID("03")%>" /><label for="<%=randID("23")%>">AG实地厅</label>
                <input type="radio" name="gameid_1" id='<%=randID("24")%>' value="<%=randID("04")%>" /><label for="<%=randID("24")%>">波音厅</label>
                <input type="radio" name="gameid_1" id="<%=randID("25")%>" value="<%=randID("05")%>" /><label for="<%=randID("25")%>">HG厅</label>
                <input type="radio" name="gameid_1" id="<%=randID("26")%>" value="<%=randID("06")%>" style="display: none;" /><label for="<%=randID("26")%>" style="display: none;">PT厅</label>
            </td>
        </tr>
        <tr>
            <td class="title1">转出额度：</td>
            <td>
                <input type="text" name="amount" /><input type="button" class="info_button" value="提交" onclick="<%=randID("f2")%>_($('#gamewithdrawal'), 'gameid_1')" /><span class="user_errmsg"></span></td>
        </tr>
    </table>
</div>
