<%@ Page Title="" Language="C#" MasterPageFile="MemberCenter.master" AutoEventWireup="true" Inherits="SitePage" %>

<%@ Register Src="01.ascx" TagPrefix="uc1" TagName="MemberCenter01" %>

<asp:Content runat="server" ContentPlaceHolderID="user_center_content"><uc1:MemberCenter01 runat="server" css_display="block" /></asp:Content>