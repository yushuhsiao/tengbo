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
 *���ܣ���ʱ���˽��׽ӿڽ���ҳ
 *�汾��3.0
 *���ڣ�2013-08-01
 *˵����
 *���´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���վ����Ҫ�����ռ����ĵ���д,
 *����һ��Ҫʹ�øô��롣�ô������ѧϰ���о��Ǹ��ӿ�ʹ�ã���Ϊ�ṩһ���ο���
 **/
namespace CSharpTestPay
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////////////////////////////////////�������//////////////////////////////////////

                //���������ַ���(��ѡ)
                string input_charset1 = Request.Form["input_charset"].ToString().Trim();

                //�ӿڰ汾(��ѡ)�̶�ֵ:V3.0
                string interface_version1 = Request.Form["interface_version"].ToString().Trim();

                //�̼Һţ����
                string merchant_code1 = Request.Form["merchant_code"].ToString().Trim();

                //��̨֪ͨ��ַ(����)
                string notify_url1 = Request.Form["notify_url"].ToString().Trim();

                //���������
                string order_amount1 = Request.Form["order_amount"].ToString().Trim();

                //�̼Ҷ�����(����)
                string order_no1 = Request.Form["order_no"].ToString().Trim();

                //�̼Ҷ���ʱ��(����)
                string order_time1 = Request.Form["order_time"].ToString().Trim();

                //ǩ����ʽ(����)
                string sign_type1 = Request.Form["sign_type"].ToString().Trim();

                //��Ʒ���(ѡ��)
                string product_code1 = Request.Form["product_code"].ToString().Trim();

                //��Ʒ������ѡ�
                string product_desc1 = Request.Form["product_desc"].ToString().Trim();

                //��Ʒ���ƣ����
                string product_name1 = Request.Form["product_name"].ToString().Trim();

                //�˿�����(ѡ��)
                string product_num1 = Request.Form["product_num"].ToString().Trim();

                //ҳ����תͬ��֪ͨ��ַ(ѡ��)
                string return_url1 = Request.Form["return_url"].ToString().Trim();

                //ҵ������(����)
                string service_type1 = Request.Form["service_type"].ToString().Trim();

                //��Ʒչʾ��ַ(ѡ��)
                string show_url1 = Request.Form["show_url"].ToString().Trim();

                //����ҵ����չ������ѡ�
                string extend_param1 = Request.Form["extend_param"].ToString().Trim();

                //����ҵ��ش�������ѡ�
                string extra_return_param1 = Request.Form["extra_return_param"].ToString().Trim();

                // ֱ��ͨ�����루ѡ�
                string bank_code1 = Request.Form["bank_code"].ToString().Trim();

                //�ͻ���IP��ѡ�
                string client_ip1 = Request.Form["client_ip"].ToString().Trim();

                /* ע  new String(����.getBytes("ISO-8859-1"),"��ҳ������ʽ"); ��ΪGBK���� ���滻UTF-8 ΪGBK*/               

                if (product_name1 != "")
                {
                    product_name1 = product_name1;
                }
                if (product_desc1 != "")
                {
                    product_desc1 = product_desc1;
                }
                if (extend_param1 != "")
                {
                    extend_param1 = extend_param1;
                }
                if (extra_return_param1 != "")
                {
                    extra_return_param1 = extra_return_param1;
                }
                if (product_code1 != "")
                {
                    product_code1 = product_code1;
                }
                if (return_url1 != "")
                {
                    return_url1 = return_url1;
                }
                if (show_url1 != "")
                {
                    show_url1 = show_url1;
                }

                /*
                **
                 ** ǩ��˳���ղ�����a��z��˳��������������ͬ����ĸ���򿴵ڶ�����ĸ���Դ����ƣ�ͬʱ���̼�֧����Կkey����������ǩ����
                 ** ��ɹ������£�
                 ** ������1=����ֵ1&������2=����ֵ2&����&������n=����ֵn&key=keyֵ
                 **/

                string signSrc= "";

	            //��֯������Ϣ
	            if(bank_code1!="") {
		            signSrc=signSrc+"bank_code="+bank_code1+"&";
	            }
	            if(client_ip1!="") {
		            signSrc=signSrc+"client_ip="+client_ip1+"&";
	            }
	            if(extend_param1!="") {
		            signSrc=signSrc+"extend_param="+extend_param1+"&";
	            }
	            if(extra_return_param1!="") {
		            signSrc=signSrc+"extra_return_param="+extra_return_param1+"&";
	            }
	            if (input_charset1!="") {
		            signSrc=signSrc+"input_charset="+input_charset1+"&";
	            }
	            if (interface_version1!="") {
		            signSrc=signSrc+"interface_version="+interface_version1+"&";
	            }
	            if (merchant_code1!="") {
		            signSrc=signSrc+"merchant_code="+merchant_code1+"&";
	            }
	            if(notify_url1!="") {
		            signSrc=signSrc+"notify_url="+notify_url1+"&";
	            }
	            if(order_amount1!="") {
		            signSrc=signSrc+"order_amount="+order_amount1+"&";
	            }
	            if(order_no1!="") {
		            signSrc=signSrc+"order_no="+order_no1+"&";
	            }
	            if(order_time1!="") {
		            signSrc=signSrc+"order_time="+order_time1+"&";
	            }
	            if(product_code1!="") {
		            signSrc=signSrc+"product_code="+product_code1+"&";
	            }
	            if(product_desc1!="") {
		            signSrc=signSrc+"product_desc="+product_desc1+"&";
	            }
	            if(product_name1!="") {
		            signSrc=signSrc+"product_name="+product_name1+"&";
	            }
	            if(product_num1!="") {
		            signSrc=signSrc+"product_num="+product_num1+"&";
	            }
	            if(return_url1!="") {
		            signSrc=signSrc+"return_url="+return_url1+"&";
	            }
	            if(service_type1!="") {
		            signSrc=signSrc+"service_type="+service_type1+"&";
	            }
	            if(show_url1!="") {
		            signSrc = signSrc+"show_url="+show_url1+"&";
	            }

                //������Կ
                string key = "123456789a123456789_";
                signSrc = signSrc + "key=" + key;

                string singInfo = signSrc;
                //Response.Write("singInfo=" + singInfo + "<br>");


                //ǩ��
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
                string sign1 = BitConverter.ToString(MD5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(singInfo))).Replace("-", "").ToLower();
                //Response.Write("sign1=" + sign1 + "<br>");

                sign.Value = sign1;
                merchant_code.Value = merchant_code1;
                bank_code.Value = bank_code1;
                order_no.Value = order_no1;
                order_amount.Value = order_amount1;
                service_type.Value = service_type1;
                input_charset.Value = input_charset1;
                notify_url.Value = notify_url1;
                interface_version.Value = interface_version1;
                sign_type.Value = sign_type1;
                order_time.Value = order_time1;
                product_name.Value = product_name1;
                client_ip.Value = client_ip1;
                extend_param.Value = extend_param1;
                extra_return_param.Value = extra_return_param1;
                product_code.Value = product_code1;
                product_desc.Value = product_desc1;
                product_num.Value = product_num1;
                return_url.Value = return_url1;
                show_url.Value = show_url1;


            }
            finally
            {

            }
        }
    }
}