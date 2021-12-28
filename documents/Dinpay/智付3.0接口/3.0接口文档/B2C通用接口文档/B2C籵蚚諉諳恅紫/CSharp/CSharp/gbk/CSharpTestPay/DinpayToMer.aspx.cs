using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
/* *
 ���ܣ��Ǹ�ҳ����תͬ��֪ͨҳ��
 �汾��3.0
 ���ڣ�2013-08-01
 ˵����
 ���´����Ϊ�˷����̻���װ�ӿڶ��ṩ����������˵�����ĵ�Ϊ׼���̻����Ը����Լ���վ����Ҫ�����ռ����ĵ���д��
 * */
namespace CSharpTestPay
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //��ȡ�Ǹ�GET����������Ϣ
            //�̺ź�
            string merchant_code = Request.Form["merchant_code"].ToString().Trim();

            //֪ͨ����
            string notify_type = Request.Form["notify_type"].ToString().Trim();

            //֪ͨУ��ID
            string notify_id = Request.Form["notify_id"].ToString().Trim();

            //�ӿڰ汾
            string interface_version = Request.Form["interface_version"].ToString().Trim();

            //ǩ����ʽ
            string sign_type = Request.Form["sign_type"].ToString().Trim();

            //ǩ��
            string dinpaySign = Request.Form["sign"].ToString().Trim();

            //�̼Ҷ�����
            string order_no = Request.Form["order_no"].ToString().Trim();

            //�̼Ҷ���ʱ��
            string order_time = Request.Form["order_time"].ToString().Trim();

            //�̼Ҷ������
            string order_amount = Request.Form["order_amount"].ToString().Trim();

            //�ش�����
            string extra_return_param = Request.Form["extra_return_param"].ToString().Trim();

            //�Ǹ����׶�����
            string trade_no = Request.Form["trade_no"].ToString().Trim();

            //�Ǹ�����ʱ��
            string trade_time = Request.Form["trade_time"].ToString().Trim();

            //����״̬ SUCCESS �ɹ�  FAILED ʧ��
            string trade_status = Request.Form["trade_status"].ToString().Trim();

            //���н�����ˮ��
            string bank_seq_no = Request.Form["bank_seq_no"].ToString().Trim();

            /**
             *ǩ��˳���ղ�����a��z��˳��������������ͬ����ĸ���򿴵ڶ�����ĸ���Դ����ƣ�
            *ͬʱ���̼�֧����Կkey����������ǩ������ɹ������£�
            *������1=����ֵ1&������2=����ֵ2&����&������n=����ֵn&key=keyֵ
            **/


            //��֯������Ϣ
            string signStr = "";

            if (bank_seq_no != "")
            {
                signStr = signStr + "bank_seq_no=" + bank_seq_no + "&";
            }

            if (extra_return_param != "")
            {
                signStr = signStr + "extra_return_param=" + extra_return_param + "&";
            }
            signStr = signStr + "interface_version=V3.0" + "&";
            signStr = signStr + "merchant_code=" + merchant_code + "&";


            if (notify_id != "")
            {
                signStr = signStr + "notify_id=" + notify_id + "&notify_type=page_notify&";
            }

            signStr = signStr + "order_amount=" + order_amount + "&";
            signStr = signStr + "order_no=" + order_no + "&";
            signStr = signStr + "order_time=" + order_time + "&";
            signStr = signStr + "trade_no=" + trade_no + "&";
            signStr = signStr + "trade_status=" + trade_status + "&";

            if (trade_time != "")
            {
                signStr = signStr + "trade_time=" + trade_time + "&";
            }

            string key = "123456789a123456789_";

            signStr = signStr + "key=" + key;
            string signInfo = signStr;

            //����װ�õ���ϢMD5ǩ��
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            string sign = BitConverter.ToString(MD5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(signInfo))).Replace("-", "").ToLower();

            //�Ƚ��Ǹ����ص�ǩ�������̼������װ��ǩ�����Ƿ�һ��
            if (dinpaySign == sign)
            {
                //��ǩ�ɹ�
                /**

                �˴������̻�ҵ�����

                ҵ�����
                */
            }
            else
            {
                //��ǩʧ�� ҵ�����
            }
        }
    }
}