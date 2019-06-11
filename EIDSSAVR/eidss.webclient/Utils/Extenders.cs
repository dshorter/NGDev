﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using bv.common.Configuration;
using bv.common.Resources;
using bv.model.BLToolkit;
using bv.model.Model.Core;
using System.Web;
using System.Collections;
using bv.model.Helpers;
using eidss.model.Schema;
using eidss.web.common.Utils;
using bv.common.Enums;
using eidss.webclient.Configurations;
using eidss.model.Core;
using bv.common.Core;
using System.Globalization;

//using Kendo.Mvc.UI;

namespace eidss.webclient.Utils
{
    
    public static partial class Extenders
    {
        private static readonly int m_HeartbeatSeconds = Config.GetIntSetting("HeartbeatSeconds", 10);
        private static readonly int m_LifetimeSeconds = Config.GetIntSetting("LifetimeSeconds", 1200);

        #region IEnumerable extenders

        /// <summary>
        /// Method from forum
        /// http://social.msdn.microsoft.com/forums/en-US/adodotnetentityframework/thread/f7cb280d-17c4-4cb3-8b8c-82bf46ea4c56
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string propertyName, bool ascending)
        {
            ParameterExpression prm = Expression.Parameter(typeof(T), "it");
            Expression property = Expression.Property(prm, propertyName);
            Type propertyType = property.Type;
            MethodInfo method = typeof(Extenders).GetMethod("OrderByProperty", BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(typeof(T), propertyType);

            return (IEnumerable<T>)method.Invoke(null, new object[] { query, prm, property, ascending });
        }

        private static IEnumerable<T> OrderByProperty<T, P>(this IEnumerable<T> query, ParameterExpression prm, Expression property, bool ascending)
        {
            Func<IEnumerable<T>, Func<T, P>, IEnumerable<T>> orderBy = (q, p) => ascending ? q.OrderBy(p) : q.OrderByDescending(p);
            return orderBy(query, Expression.Lambda<Func<T, P>>(property, prm).Compile());
        }

        #endregion

        #region Controller extenders

        public static string RenderPartialView(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

        public static IHtmlString RenderEidss(params string[] paths)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionCode = string.Format("-{0}-{1}-{2}-{3}", version.Major, version.Minor, version.Build, version.Revision);
            paths = paths.Select(c => c + versionCode).ToArray();
            return System.Web.Optimization.Scripts.Render(paths);
        }
        
        public static MvcHtmlString IncludeVersionedJs(this HtmlHelper helper, string filename)
        {
            string versionCode = VersionCode();
            var path = "/s/" + versionCode + UrlHelper.GenerateContentUrl(filename, helper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<script type='text/javascript' src='" + path + "'></script>");
        }
        public static MvcHtmlString IncludeVersionedCss(this HtmlHelper helper, string filename)
        {
            string versionCode = VersionCode();
            var path = "/s/" + versionCode + UrlHelper.GenerateContentUrl(filename, helper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<link href='" + path + " 'rel='stylesheet' type='text/css' />");
        }
        public static string VersionCode()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionCode = string.Format("{0}-{1}-{2}-{3}", version.Major, version.Minor, version.Build, version.Revision);
            return versionCode;
        }


        public static MvcHtmlString IdentificationAndHeartbeat(this HtmlHelper htmlHelper, string keyName, long keyValue)
        {
            return new MvcHtmlString(
                @"
<input id='" + keyName + @"' name='" + keyName + @"' type='hidden' value='" + keyValue + @"' />
<input id='hdnIdentField' type='hidden' value='" + keyValue + @"' />
<script type='text/javascript'>
    var lifetime = " + m_LifetimeSeconds * 1000 + @";
    var timeout = " + m_HeartbeatSeconds * 1000 + @";
    var objId = " + keyValue + @";
    var initialTime = 0;
    setInterval(
        function () {
            $.bvPost(bvUrls.getHeartbeatUrl(), { id: objId }, 
                function (data) {
                    initialTime = 0;
                    if (data.result == 1) {
                        actions.redirect(bvUrls.getTimeoutUrl());
                    } else if (data.result == 2) {
                        actions.redirect(bvUrls.getIncomlianceUrl() + '?url=' + data.url);
                    }
                }, function () {
                    initialTime += timeout;
                    if (initialTime >= lifetime) {
                        actions.redirect(bvUrls.getTimeoutUrl());
                    }
                });
        }, timeout);
</script>
                ");
        }

        public static MvcHtmlString Heartbeat(this HtmlHelper htmlHelper)
        {
            return new MvcHtmlString(
                @"
<script type='text/javascript'>
    var lifetime = " + m_LifetimeSeconds * 1000 + @";
    var timeout = " + m_HeartbeatSeconds * 1000 + @";
    var objId = -1;
    var initialTime = 0;
    setInterval(
        function () {
            $.bvPost(bvUrls.getHeartbeatUrl(), { id: objId }, 
                function (data) {
                    initialTime = 0;
                    if (data.result == 1) {
                        actions.redirect(bvUrls.getTimeoutUrl());
                    } else if (data.result == 2) {
                        actions.redirect(bvUrls.getIncomlianceUrl() + '?url=' + data.url);
                    }
                }, function () {
                    initialTime += timeout;
                    if (initialTime >= lifetime) {
                        actions.redirect(bvUrls.getTimeoutUrl());
                    }
                });
        }, timeout);
</script>
                ");
        }

        private static string BindToProp(Expression expr)
        {
            return expr is MemberExpression
                       ? ((MemberExpression) expr).Member.Name
                       : expr is UnaryExpression
                             ? BindToProp((expr as UnaryExpression).Operand)
                             : "";
        }

        private static string BindToProp<O>(Expression<Func<O, object>> expr)
            where O : IObject
        {
            return BindToProp(expr.Body);
        }

        #region BV UI elements

        public static ComboBoxBuilder BvComboboxWithList<O>
            (this HtmlHelper htmlHelper,
             O obj, Expression<Func<O, object>> expr, BvSelectList items, string keyName = null, string valueName = null)
            where O : IObject
        {
            return BvCombobox(htmlHelper, obj, expr, true, null, null, null, false, items, keyName, valueName);
        }

        public static ComboBoxBuilder BvCombobox<O>(this HtmlHelper htmlHelper, 
            O obj, Expression<Func<O, object>> expr, bool bBind = true, string clientOnChange = null, 
            string styleClass = null, string strRight = null, bool readOnly = false, BvSelectList items = null,
            string keyName = null, string valueName = null, EditorControlWidth width = EditorControlWidth.Normal,
            string textName = null, CachedLookup cacheKey = CachedLookup.None)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            var cls = String.Empty; // default style - width:100%
            var stl = String.Empty;
            if (!string.IsNullOrEmpty(styleClass))
            {
                cls = styleClass;
            }
            if (obj.IsRequired(bindToProp))
            {
                cls += " requiredField";
            }
            if (obj.IsInvisible(bindToProp))
            {
                cls += " invisible";
                stl = "display: none;";
            }
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            var htmlAttributes = bIsAutoTestingVersion
                ? (object) new { @class = cls, style = stl, bvid = obj.ObjectName + "." + bindToProp }
                : new { @class = cls, style = stl };
            //if (obj.IsHiddenPersonalData(bindToProp))
            //{                
            //    return new MvcHtmlString(
            //        String.Format("<div class='tdContainer'>{0}</div>", 
            //        htmlHelper.TextBox(bindToProp, "***********", 
            //        new Dictionary<string,object> { {"class", cls +"readonlyField"} })));

            //}

            var selectlist = items ?? obj.GetList(bindToProp);
            var index = selectlist == null ? -1 : selectlist.items.IndexOf(selectlist.selectedValue);
            if (index < 0 && selectlist != null && selectlist.items != null && selectlist.selectedValue != null)
            {
                var str = selectlist.selectedValue.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    for (var i = 0; i < selectlist.items.Count; i++)
                    {
                        if (selectlist.items[i].ToString() == str)
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }

            var bEnable = !(obj.IsReadOnly(bindToProp) || readOnly);
            var permission = obj.GetPermissions();
            if (permission != null)
            {
                bEnable = bEnable && !permission.IsReadOnlyForEdit;
            }
            if (strRight != null)
            {
                bEnable = bEnable && ModelUserContext.Instance.CanDo(strRight);
            }
            
            var ret = htmlHelper.Kendo().ComboBox()
                .Name(obj.ObjectIdent + bindToProp)
                .SelectedIndex(index)
                .Enable(bEnable)
                .Filter("startswith")
                .HtmlAttributes(htmlAttributes);
            if (bBind && selectlist != null)
            {
                ret =
                    ret.BindTo(new SelectList(selectlist.items
                                            , selectlist.dataValueField
                                            , valueName ?? selectlist.dataTextField
                                            , selectlist.selectedValue));
            }
            if (bBind)
            {
                ret.Events(events => 
                {
                    events
                        //.Change("bvComboBox.onTextChanged")
                        //.Select(clientOnChange ?? "bvComboBox.onChanged")
                        .Change(clientOnChange ?? "bvComboBox.onChanged")
                        .Open("function(e) { bvComboBox.onOpen(e, " + (int)width + "); }");
                    if (bIsAutoTestingVersion)
                        events.DataBound("bvComboBox.onDataBound");
                });
            }
            else
            {
                ret = ret
                    .DataSource(source =>
                        source.Read(read =>
                            {
                                if (cacheKey == CachedLookup.None)
                                    read.Action("SelectGeneric", "System", new { id = (long) obj.Key, lookup = bindToProp, keyname = keyName, textname = textName });
                                else
                                    read.Action(cacheKey.ToString(), "SystemCached");
                            })
                            .ServerFiltering(false))
                    .DataValueField("id")
                    .DataTextField("name")
                    .Events(events =>
                    {
                        events
                            .Change(clientOnChange ?? "bvComboBox.onChanged")
                            .Open("function(e) { bvComboBox.onOpen(e, " + (int)width + "); }");
                        if (bIsAutoTestingVersion)
                            events.DataBound("bvComboBox.onDataBound");
                    });
            }
            return ret;
        }

        public static Kendo.Mvc.UI.Fluent.ComboBoxBuilder BvDummyCombobox(this HtmlHelper htmlHelper)
        {
            string style = "width:0px;";
            var htmlAttributes = new { style };
            var ret = htmlHelper.Kendo().ComboBox()
                .Name("dummy_combo")
                .Enable(false)
                .HtmlAttributes(htmlAttributes);
            return ret;
        }


        public static DropDownListBuilder ComboboxWithCheckboxes
            (this HtmlHelper htmlHelper,
                string ctlName, long key, string keyName, string textName, string selected, 
                object htmlAttributes,
                bool enabled = true, BvSelectList items = null,
                string controller = null, string action = null, EditorControlWidth width = EditorControlWidth.Normal
            )
        {
            var ret = htmlHelper.Kendo().DropDownList()
                .Name(ctlName)
                .DataTextField("Text")
                .DataValueField("Value")
                .Template("<div class='${data.classname}'><input type='checkbox' checkedname='${data.Text}' group='${data.group}' value='${data.Value}' class='check-item' id='chb${data.Value}'/><span>${data.Text}</span></div>")
                .Events(e => e.Select("bvCheckedComboBox.onCheckedComboBoxChanged")
                              .DataBound("function(e) { bvCheckedComboBox.bindCheckedComboBoxClickEvent('" + ctlName + "', '" + selected + "');}")
                              .Open("function(e) { bvComboBox.onOpen(e, " + (int)width + ", true); }"))
                .Enable(enabled)
                .HtmlAttributes(htmlAttributes);

            if (controller != null && action != null)
            {
                ret = ret.DataSource(source => source.Read(read => read.Action(action, controller, new { id = key, keyname = keyName, valuename = textName })));
            }
            else if(items!=null)
            {
                ret = ret.BindTo(new SelectList(items.items, keyName, textName, null));
            }

            return ret;
            
        }

        public static DropDownListBuilder BvComboboxWithCheckboxes<O>(this HtmlHelper htmlHelper,
            O obj, Expression<Func<O, object>> expr, string keyName, string textName, string selected,
            /*bool bBind = true, string clientOnChange = null, */
            string styleClass = null, string strRight = null, bool readOnly = false, BvSelectList items = null,
            string controller = null, string action = null, EditorControlWidth width = EditorControlWidth.Normal
            )
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            var cls = String.Empty; // default style - width:100%
            if (!string.IsNullOrEmpty(styleClass))
            {
                cls = styleClass;
            }
            if (obj.IsRequired(bindToProp))
            {
                cls += " requiredField";
            }
            if (obj.IsInvisible(bindToProp))
            {
                cls += " invisible";
            }
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            var htmlAttributes = bIsAutoTestingVersion
                ? (object)new { @class = cls, bvid = obj.ObjectName + "." + bindToProp }
                : new { @class = cls };

            var selectlist = items ?? obj.GetList(bindToProp);

            var bEnable = !(obj.IsReadOnly(bindToProp) || readOnly);
            var permission = obj.GetPermissions();
            if (permission != null)
            {
                bEnable = bEnable && !permission.IsReadOnlyForEdit;
            }
            if (strRight != null)
            {
                bEnable = bEnable && ModelUserContext.Instance.CanDo(strRight);
            }

            return ComboboxWithCheckboxes(htmlHelper, obj.ObjectIdent + bindToProp, (long)obj.Key, keyName, textName, selected, htmlAttributes, bEnable, selectlist, controller, action, width);
            //var ret = htmlHelper.Kendo().DropDownList()
            //    .Name(obj.ObjectIdent + bindToProp)
            //    .DataTextField("Text")
            //    .DataValueField("Value")
            //    .Template("<div class='${data.classname}'><input type='checkbox' checkedname='${data.Text}' group='${data.group}' value='${data.Value}' class='check-item' id='chb${data.Value}'/><span>${data.Text}</span></div>")
            //    .Events(e => e.Select("bvCheckedComboBox.onCheckedComboBoxChanged")
            //                  .DataBound("function(e) { bvCheckedComboBox.bindCheckedComboBoxClickEvent('" + obj.ObjectIdent + bindToProp + "', '" + selected + "');}")
            //                  .Open("function(e) { bvComboBox.onOpen(e, " + (int)width + ", true); }"))
            //    .Enable(bEnable)
            //    .HtmlAttributes(htmlAttributes);

            //if (controller != null && action != null)
            //{
            //    ret = ret.DataSource(source => source.Read(read => read.Action(action, controller, new { id = (long)obj.Key, keyname = keyName, valuename = textName })));
            //}
            //else
            //{
            //    ret = ret.BindTo(new SelectList(selectlist.items, keyName, textName, null));
            //}

            //return ret;
        }



        /*
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder BvDropdownList(this HtmlHelper htmlHelper, IObject obj, string bindToProp, bool bBind = true, string clientOnChange = null, string styleClass = null)
        {
            string cls = "fillAll"; // default style - width:100%
            if (!string.IsNullOrEmpty(styleClass))
            {
                cls = styleClass;
            }
            if (obj.IsRequired(bindToProp))
            {
                cls += " requiredField";
            }
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            var htmlAttributes = bIsAutoTestingVersion
                ? (object)new { @class = cls, bvid = obj.ObjectName + "." + bindToProp }
                : new { @class = cls };
            BvSelectList selectlist = obj.GetList(bindToProp);

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;

            var ret = htmlHelper.Kendo().DropDownList()
                .Name(obj.ObjectIdent + bindToProp)
                .SelectedIndex(selectlist.items.IndexOf(selectlist.selectedValue) < 0 ? 0 : selectlist.items.IndexOf(selectlist.selectedValue))
                .Enable(bEnable)
                .Events(events => events.Change(clientOnChange ?? "bvComboBox.applyChanges"))
                .HtmlAttributes(htmlAttributes);
            if (bBind)
                ret = ret.BindTo(new SelectList(selectlist.items, selectlist.dataValueField, selectlist.dataTextField, selectlist.selectedValue));
            return ret;
        }
        */

        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder BvDatebox<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, string clientOnChange = null, bool limitMonth = false)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            string cls = "";

            if (obj.IsInvisible(bindToProp) && obj.IsRequired(bindToProp))
            {
                cls = "requiredField invisible";
            }
            else if (obj.IsRequired(bindToProp))
            {
                cls = "requiredField";
            }
            else if (obj.IsInvisible(bindToProp))
            {
                cls = "invisible";
            }

            //if(EidssSiteContext.Instance.IsGeorgiaCustomization)
            //{
            //    cls = "requiredField";
            //}

            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            //TODO: Chrome date problem fix - start
            //old version//var htmlAttributes = bIsAutoTestingVersion
            //old version//    ? (object)new { @class = cls, bvid = obj.ObjectName + "." + bindToProp }
            //old version//    : new { @class = cls };
            var htmlAttributes = bIsAutoTestingVersion
                ? (object)new { @class = cls, type = "text", bvid = obj.ObjectName + "." + bindToProp }
                : new { @class = cls, type = "text" };
            //TODO: Chrome date problem fix - end

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;


            var val = obj.GetValue(bindToProp) as DateTime?;
            limitMonth &= (val != null);
            DateTime MaxMonth = DateTime.MaxValue;
            DateTime MinMonth = DateTime.MinValue;
            if(limitMonth){
                MinMonth = val.Value.AddDays(-val.Value.Day+1);
                MaxMonth = MinMonth.AddMonths(1).AddMinutes(-1);
                if(MaxMonth > DateTime.Today.AddDays(1).AddMinutes(-1))
                    MaxMonth = DateTime.Today.AddDays(1).AddMinutes(-1);
            }

            var ret = htmlHelper.Kendo().DatePicker()
                .Name(obj.ObjectIdent + bindToProp)
                .Value(val)
                .Events(events => events
                    .Change(clientOnChange ?? "formRefresher.onDateChanged")
                    //.Open("datePickerFacade.onOpen")
                    )
                //.Footer(Localizer.CurrentCultureLanguageID != Localizer.lngThai)
                .HtmlAttributes(htmlAttributes)
                .Enable(bEnable)
                .Max(MaxMonth)
                .Min(MinMonth);

            return ret;
        }

        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder BvDateboxDOB<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, Expression<Func<O, object>> expr2, string clientOnChange = null, bool limitMonth = false)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            var bindToProp2 = BindToProp(expr2);
            string cls = "";

            if (obj.IsInvisible(bindToProp) && obj.IsRequired(bindToProp))
            {
                cls = "requiredField invisible";
            }
            else if (obj.IsRequired(bindToProp))
            {
                cls = "requiredField";
            }
            else if (obj.IsInvisible(bindToProp))
            {
                cls = "invisible";
            }

            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            //TODO: Chrome date problem fix - start
            //old version//var htmlAttributes = bIsAutoTestingVersion
            //old version//    ? (object)new { @class = cls, bvid = obj.ObjectName + "." + bindToProp }
            //old version//    : new { @class = cls };
            var htmlAttributes = bIsAutoTestingVersion
                ? (object)new { @class = cls, type = "text", bvid = obj.ObjectName + "." + bindToProp }
                : new { @class = cls, type = "text" };
            //TODO: Chrome date problem fix - end

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;


            var val = obj.GetValue(bindToProp) as DateTime?;
            var val2 = obj.GetValue(bindToProp2) as Int32?;
            limitMonth &= (val != null);
            DateTime MaxMonth = DateTime.MaxValue;
            DateTime MinMonth = DateTime.MinValue;
            if (limitMonth)
            {
                MinMonth = val.Value.AddDays(-val.Value.Day + 1);
                MaxMonth = MinMonth.AddMonths(1).AddMinutes(-1);
                if (MaxMonth > DateTime.Today.AddDays(1).AddMinutes(-1))
                    MaxMonth = DateTime.Today.AddDays(1).AddMinutes(-1);
            }
            
            if(val != null)
            {
                int x = DateTime.Today.Year - val.Value.Year;

                if (x >= 60 && val.HasValue && Localizer.CurrentCultureLanguageID == Localizer.lngThai)
                {
                    try
                    {
                        val = val.Value.AddYears(-543);
                    }
                    catch (Exception ex)
                    {
                        string e = ex.Message;
                    }
                }
            }

            var ret = htmlHelper.Kendo().DatePicker()
                .Name(obj.ObjectIdent + bindToProp)
                .Value(val)
                .Events(events => events
                    .Change(clientOnChange ?? "formRefresher.onDateChanged")
                    //.Open("datePickerFacade.onOpen")
                    )
                //.Footer(Localizer.CurrentCultureLanguageID != Localizer.lngThai)
                .HtmlAttributes(htmlAttributes)
                .Enable(bEnable)
                .Max(MaxMonth)
                .Min(MinMonth);

            return ret;
        }

        /*
        public static Kendo.Mvc.UI.Fluent.DateTimePickerBuilder BvDatetimebox<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, string clientOnChange = null)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            string cls = "";
            if (obj.IsRequired(bindToProp))
            {
                cls = "requiredField";
            }
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            var htmlAttributes = bIsAutoTestingVersion
                ? (object)new { @class = cls, bvid = obj.ObjectName + "." + bindToProp }
                : new { @class = cls };

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;

            var ret = htmlHelper.Kendo().DateTimePicker()
                .Name(obj.ObjectIdent + bindToProp)
                .Value(obj.GetValue(bindToProp) as DateTime?)
                .Events(events => events.Change(clientOnChange ?? "formRefresher.onDateTimeChanged"))
                .HtmlAttributes(htmlAttributes)
                .Enable(bEnable)
                .Max(DateTime.Now.AddHours(1));
            return ret;
        }
        */
        public static Kendo.Mvc.UI.Fluent.DateTimePickerBuilder BvDummyDatetimebox(this HtmlHelper htmlHelper)
        {
            string style = "width:0px;";

            //TODO: Chrome date problem fix - start
            //old version//var htmlAttributes = new { style };
            var htmlAttributes = new { style, type = "text" };
            //TODO: Chrome date problem fix - end

            var ret = htmlHelper.Kendo().DateTimePicker()
                .Name("dummy_datetime")
                .Enable(false)
                .HtmlAttributes(htmlAttributes);
            return ret;
        }

        public static MvcHtmlString BvHidden<O>(this HtmlHelper htmlHelper, O obj, string bindToProp)
            where O : IObject
        {
            return htmlHelper.Hidden(obj.ObjectIdent + bindToProp, obj.GetValue(bindToProp));
        }
        public static MvcHtmlString BvHidden<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            return htmlHelper.Hidden(obj.ObjectIdent + bindToProp, obj.GetValue(bindToProp));
        }
        public static MvcHtmlString BvLabel<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, Expression<Func<O, object>> exprForName = null)
            where O : IObject
        {
            var labelFor = BindToProp(expr);
            string labelForName = obj.ObjectIdent + (exprForName == null ? labelFor : BindToProp(exprForName));
            var label = new TagBuilder("label");
            label.Attributes["for"] = TagBuilder.CreateSanitizedId(labelForName);
            var labelText = obj.GetType().GetProperty(labelFor) == null ? null 
                : obj.GetType().GetProperty(labelFor).GetCustomAttributes(typeof(LocalizedDisplayNameAttribute), true).SingleOrDefault(c => true, c => (c as LocalizedDisplayNameAttribute).DisplayName);
            string text = labelText ?? Translator.GetString(labelFor);
            label.InnerHtml = HttpUtility.HtmlEncode(text);
            if (obj.IsInvisible(labelFor))
            {
                label.AddCssClass("invisible");
            }
            return MvcHtmlString.Create(label.ToString());
        }

        public static MvcHtmlString BvHiddenPersonalData<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, string format = "**********", bool isInvisible = false)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            return BvHiddenPersonalData(htmlHelper, obj, bindToProp, format, isInvisible);
        }
        public static MvcHtmlString BvHiddenPersonalData<O>(this HtmlHelper htmlHelper, O obj, string bindToProp, string format = "**********", bool isInvisible = false)
            where O : IObject
        {
            var htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("readonly", "readonly");
            htmlAttributes.Add("class", "readonlyField");
            if (isInvisible)
            {
                htmlAttributes["class"] = "readonlyField invisible";
            }
            return new MvcHtmlString(String.Format("<span class='tdContainer'>{0}</span>", htmlHelper.TextBox(bindToProp, format, htmlAttributes)));
        }

        public static MvcHtmlString BvEditbox<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, bool refreshOnLostFocus = false, string width = null, string strRight = null,
            string clientScript = null, int isNumeric = 0, int maxLength = 0)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            return BvEditbox(htmlHelper, obj, bindToProp, refreshOnLostFocus, width, strRight, clientScript, isNumeric, maxLength);
        }
        public static MvcHtmlString BvEditbox<O>(this HtmlHelper htmlHelper, O obj, string bindToProp, bool refreshOnLostFocus = false, string width = null, string strRight = null,
            string clientScript = null, int isNumeric = 0, int maxLength = 0)
            where O : IObject
        {
            if (obj.IsHiddenPersonalData(bindToProp))
            {
                return htmlHelper.BvHiddenPersonalData(obj, bindToProp, isInvisible: obj.IsInvisible(bindToProp));
            }

            var htmlAttributes = new Dictionary<string, object>();
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            if (bIsAutoTestingVersion)
                htmlAttributes.Add("bvid", obj.ObjectName + "." + bindToProp);

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
            {
                bEnable = bEnable && !permission.IsReadOnlyForEdit;
            }
            if (strRight != null)
            {
                bEnable = bEnable && ModelUserContext.Instance.CanDo(strRight);
            }

            if (!bEnable)
            {
                htmlAttributes.Add("readonly", "readonly");
            }
            if (!string.IsNullOrEmpty(width))
            {
                htmlAttributes.Add("style", String.Format("width:{0}", width));
            }
            var classes = new StringBuilder();
            if (!bEnable)
            {
                classes.Append("readonlyField ");
            }
            if (obj.IsRequired(bindToProp))
            {
                classes.Append("requiredField ");
            }
            if (obj.IsInvisible(bindToProp))
            {
                classes.Append("invisible ");
            }
            if (classes.Length > 0)
            {
                htmlAttributes.Add("class", classes.ToString().Trim());
            }
            int? maxSize = ((IObjectMeta)obj.GetAccessor()).MaxSize(bindToProp);
            if (maxSize.HasValue || maxLength > 0)
            {
                htmlAttributes.Add("maxlength", maxSize.HasValue ? maxSize : maxLength);
            }
            if (refreshOnLostFocus)
            {
                string textBoxName = obj.ObjectIdent + bindToProp;
                htmlAttributes.Add("onblur", string.Format("{0}('{1}');", clientScript ?? "formRefresher.onTextBoxChanged", textBoxName));
            }
            if (isNumeric == 1)
            {
                htmlAttributes.Add("onkeypress", "return isNumber(event)");
            }
            else if (isNumeric == 2)
            {
                htmlAttributes.Add("onkeypress", "return isNumberOrABC(event)");
            }
            MvcHtmlString textBox = htmlHelper.TextBox(obj.ObjectIdent + bindToProp, obj.GetValue(bindToProp), htmlAttributes);
            string result = String.Format("<span class='tdContainer'>{0}</span>", textBox); // fix for IE (100% 'input' width)
            return new MvcHtmlString(result);
        }
        public static MvcHtmlString BvReadOnlyEditbox<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            var htmlAttributes = new Dictionary<string, object>();
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            if (bIsAutoTestingVersion)
                htmlAttributes.Add("bvid", obj.ObjectName + "." + bindToProp);
            htmlAttributes.Add("readonly", "readonly");
            htmlAttributes.Add("class", "readonlyField");
            MvcHtmlString textBox = htmlHelper.TextBox(obj.ObjectIdent + bindToProp, obj.GetValue(bindToProp), htmlAttributes);
            string result = String.Format("<div class='tdContainer'>{0}</div>", textBox); // fix for IE (100% 'input' width)
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString BvTextArea<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, bool refreshOnLostFocus = false)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            if (obj.IsHiddenPersonalData(bindToProp))
            {
                return htmlHelper.BvHiddenPersonalData(obj, expr);
            }

            var htmlAttributes = new Dictionary<string, object>();
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            if (bIsAutoTestingVersion)
                htmlAttributes.Add("bvid", obj.ObjectName + "." + bindToProp);

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;

            if (!bEnable)
            {
                htmlAttributes.Add("readonly", "readonly");
            }
            if (obj.IsInvisible(bindToProp) && !bEnable)
            {
                htmlAttributes.Add("class", "invisible readonlyField");
            }
            else if (obj.IsInvisible(bindToProp))
            {
                htmlAttributes.Add("class", "invisible");
            }
            else if (!bEnable)
            {
                htmlAttributes.Add("class", "readonlyField");
            }
            string textBoxName = obj.ObjectIdent + bindToProp;
            int? maxSize = ((IObjectMeta)obj.GetAccessor()).MaxSize(bindToProp);
            if (maxSize.HasValue)
            {
                htmlAttributes.Add("maxlength", maxSize);
                htmlAttributes.Add("onkeyup", string.Format("LimitTextArea('{0}');", textBoxName));
            }
            if (refreshOnLostFocus)
            {
                htmlAttributes.Add("onblur", string.Format("formRefresher.onTextBoxChanged('{0}');", textBoxName));
            }
            var valobj = obj.GetValue(bindToProp);
            var val = valobj == null ? "" : valobj.ToString();
            MvcHtmlString textBox = htmlHelper.TextArea(obj.ObjectIdent + bindToProp, val, htmlAttributes);
            string result = String.Format("<div class='tdContainer'>{0}</div>", textBox); // fix for IE (100% 'input' width)
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString BvNumeric<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, int decimalDigits = 2, double minValue = 0, double maxValue = (double)Int32.MaxValue, bool refreshOnLostFocus = false, string clientOnChange = null)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            if (obj.IsHiddenPersonalData(bindToProp))
            {
                return htmlHelper.BvHiddenPersonalData(obj, expr, decimalDigits == 0 ? "****" : "***.**");
            }
            string cls = "";
            var htmlAttributes = new Dictionary<string, object>();
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            if (bIsAutoTestingVersion)
                htmlAttributes.Add("bvid", obj.ObjectName + "." + bindToProp);
            if (obj.IsRequired(bindToProp))
            {
                cls += "requiredField ";
            }
            if (obj.IsInvisible(bindToProp))
            {
                cls += "invisible ";
            }
            htmlAttributes.Add("class", cls);
            htmlAttributes.Add("type", "text");

            double? value = obj.GetValue(bindToProp) as double?;
            if (obj.GetValue(bindToProp) != null && decimalDigits == 0 && value == null)
                value = Convert.ToDouble((int)obj.GetValue(bindToProp));

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;

            string format = decimalDigits <= 0 ? "#" : "#.";
            for (int i = 0; i < decimalDigits; i++) format = format + "#";
            var ret = htmlHelper.Kendo()
                .NumericTextBox().Name(obj.ObjectIdent + bindToProp)
                .Format(format)
                .Value(value)
                .Enable(bEnable)
                .Decimals(decimalDigits)
                .Min(minValue)
                .Max(maxValue)
                .Placeholder("")
                .HtmlAttributes(htmlAttributes);

            if (refreshOnLostFocus)
            {
                ret = ret
                    .Events(events => events.Change(clientOnChange ?? "formRefresher.onNumericChanged"));
            }
            return new MvcHtmlString(ret.ToHtmlString());
        }
        
        public static MvcHtmlString BvRadioButtonGroup<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, bool verticalLayout = true)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            BvSelectList selectlist = obj.GetList(bindToProp);
            if (selectlist == null)
                return new MvcHtmlString("");

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;

            StringBuilder result = new StringBuilder();
            //the result is a list with radiobuttons
            result.Append(verticalLayout ? "<ul class='verticalRadioList'>" : "<ul class='horizontalRadioList'>");

            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");

            var currentValue = string.Empty;
            if (selectlist.selectedValue != null)
                currentValue = selectlist.selectedValue.ToString();
            foreach (var item in selectlist.items)
            {
                // Generate an id to be given to the radio button field 
                string value = item.GetType().GetProperty(selectlist.dataValueField).GetValue(item, null).ToString();
                if (value == "0")
                {
                    continue;
                }

                string text = item.GetType().GetProperty(selectlist.dataTextField ?? "name").GetValue(item, null).ToString();
                var id = string.Format("{0}{1}_{2}", obj.ObjectIdent, bindToProp, value);

                //// Create and populate a radio button using the existing html helpers 
                var label = htmlHelper.Label(id, HttpUtility.HtmlDecode(text));
                string radio = "";
                if (bIsAutoTestingVersion)
                    radio = String.Format("<input type='radio' id='{0}' name='{1}{2}' value='{3}' {4} {5} bvid='{6}.{2}'/>",
                            id,
                            obj.ObjectIdent,
                            bindToProp,
                            value,
                            text.Equals(currentValue) ? "checked='checked'" : "",
                            !bEnable ? "disabled='disabled'" : "",
                            obj.ObjectName
                            );
                else
                    radio = String.Format("<input type='radio' id='{0}' name='{1}{2}' value='{3}' {4} {5}/>",
                            id,
                            obj.ObjectIdent,
                            bindToProp,
                            value,
                            text.Equals(currentValue) ? "checked='checked'" : "",
                            !bEnable ? "disabled='disabled'" : ""
                            );

                result.AppendFormat("<li>{0}{1}</li>\r\n", radio, label);
            }
            result.Append("</ul>");
            return new MvcHtmlString(result.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="text"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString BvButton(this HtmlHelper htmlHelper, string text, string imageUrl)
        {
            //TODO переделать на телериковские компоненты

            var sb = new StringBuilder();
            sb.AppendFormat("<img src=\"{0}\"/>", new[] { imageUrl });
            sb.AppendFormat("<input align=\"right\" type=\"button\" value=\"{0}\"/>", new[] { text });

            return new MvcHtmlString(sb.ToString());
        }
        /*
        public static MvcHtmlString BvCheckBox<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            var result = new StringBuilder();

            bool value = obj.GetValue(bindToProp) == null ? false : (bool)obj.GetValue(bindToProp);
            var id = string.Format("{0}{1}", obj.ObjectIdent, bindToProp);

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
                bEnable = bEnable && !permission.IsReadOnlyForEdit;

            //// Create and populate a checkbox using the existing html helpers 
            var label = htmlHelper.Label(bindToProp);
            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            string radio = "";
            if (bIsAutoTestingVersion)
                radio = String.Format("<input type='checkbox' id='{0}' name='{1}' value='{2}' {3} {4} onclick='this.value=this.checked;' bvid='{5}.{6}'/>",
                                      id,
                                      id,
                                      value,
                                      value ? "checked='checked'" : "",
                                      !bEnable ? "disabled='disabled'" : "",
                                      obj.ObjectName,
                                      bindToProp
                                      );
            else
                radio = String.Format("<input type='checkbox' id='{0}' name='{1}' value='{2}' {3} {4} onclick='this.value=this.checked;'/>",
                                      id,
                                      id,
                                      value,
                                      value ? "checked='checked'" : "",
                                      !bEnable ? "disabled='disabled'" : ""
                                      );
            result.AppendFormat("<li>{0}{1}</li>\r\n", radio, label);
            return new MvcHtmlString(result.ToString());
        }
        */

        public static MvcHtmlString BvSimpleCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, IDictionary<string, object> htmlAttributes, string labelText = "")
        {
            var chk = htmlHelper.CheckBox(name, isChecked, htmlAttributes);
            var ret = chk.ToHtmlString();
            ret = ret.Replace("/><input", "/><label for='" + name + "'>" + labelText + "</label><input");
            return new MvcHtmlString(ret);
        }
        public static MvcHtmlString BvCheckBox<O>(this HtmlHelper htmlHelper, O obj, Expression<Func<O, object>> expr, bool bSendToServer, string strEventOnClick = null, string strRight = null)
            where O : IObject
        {
            var bindToProp = BindToProp(expr);
            var result = new StringBuilder();

            bool value = obj.GetValue(bindToProp) == null ? false : ParsingHelper.ParseBoolean(obj.GetValue(bindToProp).ToString());
            string checkBoxName = obj.ObjectIdent + bindToProp;

            bool bEnable = !obj.IsReadOnly(bindToProp);
            IObjectPermissions permission = obj.GetPermissions();
            if (permission != null)
            {
                bEnable = bEnable && !permission.IsReadOnlyForEdit;
            }
            if (strRight != null)
            {
                bEnable = bEnable && ModelUserContext.Instance.CanDo(strRight);
            }

            var bIsAutoTestingVersion = Config.GetBoolSetting("AutoTestingVersion");
            string radio = "";

            //// Create and populate a checkbox using the existing html helpers 
            var label = htmlHelper.BvLabel(obj, expr);
            if (bIsAutoTestingVersion)
                radio = String.Format("<input class='bvCheckbox' type='checkbox' id='{0}' name='{1}' value='{2}' {3} {4} {5} bvid='{6}.{7}'/>",
                                      checkBoxName,
                                      checkBoxName,
                                      value,
                                      value ? "checked='checked'" : "",
                                      bSendToServer ? "onclick='formRefresher.onCheckBoxChanged(\"" + checkBoxName + "\")'" :
                                        strEventOnClick == null ? "" : "onclick='" + strEventOnClick + "(\"" + checkBoxName + "\")'",
                                      !bEnable ? "disabled='disabled'" : "",
                                      obj.ObjectName,
                                      bindToProp
                                      );
            else
                radio = String.Format("<input class='bvCheckbox' type='checkbox' id='{0}' name='{1}' value='{2}' {3} {4} {5}/>",
                                      checkBoxName,
                                      checkBoxName,
                                      value,
                                      value ? "checked='checked'" : "",
                                      bSendToServer ? "onclick='formRefresher.onCheckBoxChanged(\"" + checkBoxName + "\")'" :
                                        strEventOnClick == null ? "" : "onclick='" + strEventOnClick + "(\"" + checkBoxName + "\")'",
                                      !bEnable ? "disabled='disabled'" : ""
                                      );
            result.AppendFormat("{0}{1}", radio, label);
            return new MvcHtmlString(result.ToString());
        }
        #endregion

        #region BV Grids


        private static List<Tuple<string, int>> CalculateColumnSizes<L>(GridRowSelectionType selectionType, bool bEditable, string[] strExclude, L list, List<GridModelEditableItem> editableColumns, EidssUserOptions.GridsAppearance.GridAppearance appearance)
            where L : class, IGridModelList
        {
            const int widthSelection = 46;
            const int widthEdit = 60;
            const int widthAllTable = 960;
            const int widthAddForIcons = 50;

            int length = 0;
            var ret = new List<Tuple<string, int>>();
            if (selectionType != GridRowSelectionType.None || !bEditable)
            {
                    length += widthSelection;
                    ret.Add(new Tuple<string, int>("", widthSelection));
                }
            if (bEditable)
            {
                length += widthEdit;
                ret.Add(new Tuple<string, int>("", widthEdit));
            }
            //if (!bEditable)
            //{
            //    if (selectionType != GridRowSelectionType.None)
            //    {
            //        length += widthSelection;
            //        ret.Add(new Tuple<string, int>("", widthSelection));
            //    }
            //}
            //if (bEditable)
            //{
            //    if (selectionType != GridRowSelectionType.None)
            //    {
            //        length += widthSelection;
            //        ret.Add(new Tuple<string, int>("", widthSelection));
            //    }
            //    length += widthEdit;
            //    ret.Add(new Tuple<string, int>("", widthEdit));
            //}

            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var columnTitleFont = new Font("Verdana", 12, FontStyle.Bold, GraphicsUnit.Pixel);

            int countContentColumns = 0;
            foreach (var c in list.Columns)
            {
                if (strExclude != null && strExclude.Contains(c))
                {
                    continue;
                }
                if (appearance[c].IsHidden)
                {
                    continue;
                }
                var columnTitle = Translator.GetString(list.Labels.ContainsKey(c) ? list.Labels[c] : c);
                var columnTitleSize = graphics.MeasureString(columnTitle, columnTitleFont);
                bool bWidthOverride = appearance[c].Width.HasValue;
                var columnTitleWidth = bWidthOverride ? appearance[c].Width.Value : Convert.ToInt32(columnTitleSize.Width) + widthAddForIcons;
                length += columnTitleWidth;
                if (!bWidthOverride)
                    countContentColumns++;
            }

            int addLength = 0;
            if (length < widthAllTable && countContentColumns > 0)
            {
                addLength = (widthAllTable - length) / countContentColumns;
            }

            foreach (var c in list.Columns)
            {
                ret.Add(new Tuple<string, int>(null, 0));
            }

            var tempret = new List<Tuple<string, int>>();
            foreach (var c in list.Columns)
            {
                if (strExclude != null && strExclude.Contains(c))
                {
                    continue;
                }
                var columnTitle = Translator.GetString(list.Labels.ContainsKey(c) ? list.Labels[c] : c);
                var columnTitleSize = graphics.MeasureString(columnTitle, columnTitleFont);
                bool bWidthOverride = appearance[c].Width.HasValue;
                var columnTitleWidth = bWidthOverride ? appearance[c].Width.Value : Convert.ToInt32(columnTitleSize.Width) + widthAddForIcons + addLength;

                if (editableColumns != null)
                {
                    var w = editableColumns.FirstOrDefault(i => i.Name == c, i => i.Width);
                    if (w.HasValue)
                        columnTitleWidth = Math.Max(columnTitleWidth, w.Value);
                }

                if (appearance[c].Order.HasValue)
                {
                    ret[appearance[c].Order.Value] = new Tuple<string, int>(c, columnTitleWidth);
                }
                else
                {
                    tempret.Add(new Tuple<string, int>(c, columnTitleWidth));
                }
            }

            tempret.ForEach(c => ret[ret.FindIndex(i => i.Item1 == null)] = c);

            return ret;
        }

        private static void AddNotesToTreeFFParameters(TreeViewItemFactory item, List<Section> sections, List<Parameter> parameters, long? idParent)
        {
            var sects = sections.Where(s => s.idfsParentSection == idParent);
            var pars = parameters.Where(s => s.idfsSection == idParent);

            foreach (var s in sects)
            {
                var sect = s;
                item.Add()
                    .Text(s.NationalName)
                    .ImageUrl("~/Content/Images/FlexForms/Section.png")
                    .Items(items => AddNotesToTreeFFParameters(items, sections, parameters, sect.idfsSection))
                    ;
            }

            foreach (var p in pars)
            {
                item.Add()
                    .Text(p.NationalLongName)
                    .Id(p.idfsParameter.ToString())
                    .ImageUrl("~/Content/Images/FlexForms/Parameter.png")
                    ;
            }
        }

        public static TreeViewBuilder TreeFFParameters(this HtmlHelper htmlHelper, string treeName, FFPresenterModel ff)
        {
            var tree = htmlHelper.Kendo().TreeView();
            if ((ff.CurrentTemplate != null) && ff.CurrentObservation.HasValue)
            {
                //получаем списки всех параметров и секций, которые принадлежат к этому типу формы
                var accSection = Section.Accessor.Instance(null);
                var accParameter = Parameter.Accessor.Instance(null);
                var accFormType = FormType.Accessor.Instance(null);
                List<Section> sections;
                List<Parameter> parameters;
                FormType formType;
                var idFormType = ff.CurrentTemplate.idfsFormType;
                using (var manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    sections = accSection.SelectList(manager, idFormType, null, null);
                    parameters = accParameter.SelectList(manager, null, idFormType);
                    formType = accFormType.SelectByKey(manager, idFormType);
                }
                tree = tree.Name(treeName)
                    .Checkboxes(c => c.CheckChildren(true))
                    .Items(
                    root =>
                        root.Add()
                        .Text(formType.Name)
                        .ImageUrl("~/Content/Images/FlexForms/FormType.png"))
                        .HtmlAttributes(new Dictionary<string, object> { { "style", "height: 500px;" } })
                        .Items(
                        items => AddNotesToTreeFFParameters(items, sections, parameters, null));
            }
            return tree;
        }

        public static GridBuilder<M> BvDetailsFormGrid<M, L>(this HtmlHelper htmlHelper, string gridFullName, L list,
            string styleClass, string showEditWindowFuncName = null, string clientOnSelect = null,
            bool readOnly = false, bool bCustomToolbar = false, bool bFilterable = false, bool bEditable = true, bool bToolBarBtnsDisable = false,
            string strExcludeColumns = null, string onDataBound = null, string deleteItemFuncName = "bvGrid.deleteRow", GridRowSelectionType selectionType = GridRowSelectionType.None,
            bool bDeletedOnly = false, string tooltipEdit = null, string tooltipDelete = null,
            List<GridModelEditableItem> editableColumns = null, bool bPageable = true,
            string clientOnEndEdit = null, bool bSimpleHeader = false)
            where M : class, IGridModelItem
            where L : class, IGridModelList
        {
            //TODO дописал обработку кнопок редактирования, когда bEditable = false. Почему-то пропал внутритабличный скролл.

            var gridModelFullName = typeof (L).FullName;
            var deleteAction = list.Actions.Where(c => c.Key.StartsWith("@")).SingleOrDefault(c => c.Value.ActionType == ActionTypes.Delete);
            var editAction = list.Actions.Where(c => c.Key.StartsWith("@")).SingleOrDefault(c => c.Value.ActionType == ActionTypes.Edit);
            var editorsColumnTemplate = CreateEditorsColumnTemplate(
                bEditable && !bDeletedOnly ? editAction.Value : null
                , bEditable ? deleteAction.Value : null
                , list.ListKey.ToString()
                , gridModelFullName
                , gridFullName
                , showEditWindowFuncName
                , deleteItemFuncName
                , tooltipEdit
                , tooltipDelete);
            var strExclude = strExcludeColumns == null ? null : strExcludeColumns.Split(',');
            IObjectMeta meta = list.ObjectMeta;

            var htmlAttributes = new Dictionary<string, object> {{"class", styleClass}};
            if (readOnly)
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            var parts = gridFullName.Split('_');
            var gridKey = parts.Length > 2 ? (parts[0] + "." + parts[2]) : parts[0]; 
            var gridAppearance = EidssUserContext.User.Options.Grids[gridKey];

            var widths = CalculateColumnSizes(selectionType, bEditable, strExclude, list, editableColumns, gridAppearance);
            int widthIndex = 0;

            var ret = htmlHelper.Kendo().Grid<M>()
                .Name(gridFullName)
                .Selectable(s => s.Enabled(false))
                .Sortable()
                .Scrollable(s => s.Enabled(true))
                .HtmlAttributes(htmlAttributes)
                .Resizable(r => r.Columns(true))
                .Events(events => events.Edit("function(e) { bvGrid.onEdit(e); }"))
                .Editable(s => { if (editableColumns == null) s.Enabled(false); else s.Mode(GridEditMode.InCell); })
                .Events(events => events
                    .DataBound(string.Format("function(e) {{ bvGrid.onDetailsFormGridDataBound(e, {0}) }}", string.IsNullOrEmpty(onDataBound) ? "null" : onDataBound))
                    .ColumnMenuInit("function(e) { bvGrid.onMenuInit(e, '" + gridKey + "', true); }")
                    .ColumnHide("function(e) { bvGrid.onColumnHide(e, '" + gridKey + "'); }")
                    .ColumnShow("function(e) { bvGrid.onColumnShow(e, '" + gridKey + "'); }")
                    .ColumnReorder("function(e) { bvGrid.onColumnReorder(e, '" + gridKey + "'); }")
                    .ColumnResize("function(e) { bvGrid.onColumnResize(e, '" + gridKey + "'); }")
                    )
                .DataSource(source =>
                    {
                        var ajax = source.Ajax();
                        if (bPageable)
                        {
                            ajax = ajax.PageSize(EidssUserContext.User.Options.Prefs.DetailGridPageSize);
                        }

                        ajax
                            .Read("_GridBinding", "System", new { key = list.ListKey, gridName = gridFullName, type = typeof (L).FullName })
                            .Model(model => 
                                { 
                                    model.Id(p => p.ItemKey);
                                    if (editableColumns != null)
                                    {
                                        foreach (var c in list.Columns)
                                        {
                                            if (strExclude != null && strExclude.Contains(c)) { continue; }
                                            var edit = editableColumns.FirstOrDefault(i => i.Name == c);
                                            if (edit != null)
                                            {
                                                model.Field(edit.Name, edit.Type).Editable(true);
                                            }
                                            else
                                            {
                                                model.Field<string>(c).Editable(false);
                                            }
                                        }
                                        model.Field<string>("ItemKey").Editable(false);
                                    }
                                });
                        if (editableColumns != null)
                        {
                            ajax
                                .AutoSync(true)
                                .Events(e => e
                                    //.Change("function(e) {bvGrid.onChange(e);}")
                                    //.Push("function(e) {bvGrid.onPush(e);}")
                                    .RequestEnd("function(e) {bvGrid.onRequestEnd(e);" + (clientOnEndEdit != null ? clientOnEndEdit + "(e, " + list.ListKey.ToString() + ");" : "") + "}")
                                    //.RequestStart("function(e) {bvGrid.onRequestStart(e);}")
                                    //.Sync("function(e) {bvGrid.onSync(e);}")
                                    .Error("function(e, status) {bvGrid.onError(e, status);}")
                                    )
                                .Update("_GridUpdate", "System", new { key = list.ListKey, gridName = gridFullName, type = typeof(L).FullName });
                        }
                    }
                    )
                .Columns(columns =>
                    {

                        bool bSomeFieldsAreHidden = false;
                        foreach (var columnName in widths.Where(i => !String.IsNullOrEmpty(i.Item1)).Select(i => i.Item1))
                        {
                            bSomeFieldsAreHidden |= gridAppearance[columnName].IsHidden;
                        }

                        //if (!bEditable)
                        //{
                            if (selectionType != GridRowSelectionType.None)
                            {
                                var columnTemplate = CreateSelectColumnTemplate(gridFullName, selectionType, clientOnSelect);
                                columns.Bound("ItemKey")
                                        .ClientTemplate(columnTemplate)
                                        .Sortable(false)
                                        .Title(string.Empty)
                                        .HeaderHtmlAttributes(new { @class = "checkColumn" })
                                        .HeaderTemplate(bEditable ? "" : "<div class='k-link' style='padding-bottom: 0.3em; padding-top: 0.6em; text-overflow: clip; '><img id='idSomeFieldsAreHidden' src='/Content/Images/warning12x12.png' title='" + Translator.GetBvMessageString("msgSomeFieldsAreHidden") + "' style='display: " + (bSomeFieldsAreHidden ? "inline;" : "none;") + "' /></div>")
                                        .HtmlAttributes(new { @class = "checkColumn" })
                                        .Width(widths[widthIndex++].Item2)
                                        .IncludeInMenu(false)
                                        ;
                            }
                        //}

                        if (bEditable)
                        {
                            columns.Bound("ItemKey")
                                    .ClientTemplate(string.Format(editorsColumnTemplate))
                                    .Title(string.Empty)
                                    .Sortable(false)
                                    .HeaderHtmlAttributes(new {@class = "editColumn"})
                                    .HeaderTemplate("<div class='k-link' style='padding-bottom: 0.3em; padding-top: 0.6em; text-overflow: clip; '><img id='idSomeFieldsAreHidden' src='/Content/Images/warning12x12.png' title='" + Translator.GetBvMessageString("msgSomeFieldsAreHidden") + "' style='display: " + (bSomeFieldsAreHidden ? "inline;" : "none;") + "' /></div>")
                                    .HtmlAttributes(new { @class = "editColumn" })
                                    .Width(widths[widthIndex++].Item2)
                                    .IncludeInMenu(false)
                                    ;
                        }

                        //list.Keys.ForEach(c => columns.Bound(c).Hidden());   
                        //foreach (var c in list.Columns)
                        foreach (var c in widths.Where(i => !String.IsNullOrEmpty(i.Item1)).Select(i => i.Item1))
                        {
                            bool required = meta.GridMeta.FirstOrDefault(i => i.Name == c, i => i.Required);
                            if (strExclude != null && strExclude.Contains(c))
                            {
                                continue;
                            }
                            var columnTitle = Translator.GetString(list.Labels.ContainsKey(c) ? list.Labels[c] : c);
                            //var columnTitleSize = graphics.MeasureString(columnTitle, columnTitleFont);
                            //var columnTitleWidth = Convert.ToInt32(columnTitleSize.Width) + 20;

                            /*if (list.IsHiddenPersonalData(c) && !GeoLocationsToBeHidden.Contains(c))
                            {
                                columns.Bound(c).ClientTemplate("*******").Title(columnTitle)
                                        .HeaderHtmlAttributes(new { title = columnTitle })
                                        .Width(widths[widthIndex++])
                                        ;
                            }
                            else*/ if (c.StartsWith("dat"))
                            {
                                /*if (!SystemDateFields.Contains(c))
                                {
                                    columns.Bound(c).Title(columnTitle)
                                        .HeaderHtmlAttributes(new { title = columnTitle })
                                        .ClientTemplate(String.Format("#=bvGrid.fixDateTime({0}, '{1}')#", c, bShowTime ? "{0:G}" : "{0:d}"))
                                        .Width(widths[widthIndex++])
                                        ;
                                }
                                else
                                {*/
                                if (editableColumns != null && editableColumns.Any(i => i.Name == c))
                                {
                                    columns.Bound(c).Title(columnTitle)
                                       .HeaderHtmlAttributes(new { title = columnTitle })
                                       .Format("{0:d}")
                                       .Width(widths[widthIndex++].Item2)
                                       .Hidden(gridAppearance[c].IsHidden)
                                       .IncludeInMenu(!required)
                                       ;
                                }
                                else
                                {
                                    columns.Bound(c).Title(columnTitle)
                                       .HeaderHtmlAttributes(new { title = columnTitle })
                                        //.Format(bShowTime ? "{0:G}" : "{0:d}")
                                        //.ClientTemplate(String.Format("#=bvGrid.fixDateTime({0}, '{1}')#", c, bShowTime ? "G" : "d"))
                                        //.ClientTemplate(String.Format("#=bvGrid.fixDate({0}, '{1}')#", c, bShowTime ? "G" : "d"))
                                       .ClientTemplate(String.Format("#={0} == null ? \"\" : {0}._tostring#", c))
                                       .Width(widths[widthIndex++].Item2)
                                       .Hidden(gridAppearance[c].IsHidden)
                                       .IncludeInMenu(!required)
                                       ;
                                }
                                /*}*/
                            }
                            else if (c.StartsWith("bln"))
                            {
                                columns.Bound(c).Title(columnTitle)
                                    .HeaderHtmlAttributes(new { title = columnTitle })
                                    .ClientTemplate(
                                        "<input class='bvCheckbox' type='checkbox' disabled='disabled' " +
                                                "# if (" + c + ") { #" +
                                                    "checked='checked'" +
                                                "# } #" +
                                            "/><label for='' />"
                                    )
                                    .Width(widths[widthIndex++].Item2)
                                    .Hidden(gridAppearance[c].IsHidden)
                                    .IncludeInMenu(!required)
                                    ;
                            }
                            else
                            {
                                if (editableColumns != null && editableColumns.Any(i => i.Name == c && !string.IsNullOrEmpty(i.ClientTemplate)))
                                {
                                    columns.Bound(c).Title(columnTitle)
                                        .ClientTemplate(editableColumns.FirstOrDefault(i => i.Name == c && !string.IsNullOrEmpty(i.ClientTemplate), i => i.ClientTemplate))
                                        .HeaderHtmlAttributes(new { title = columnTitle })
                                        .Width(widths[widthIndex++].Item2)
                                        .Hidden(gridAppearance[c].IsHidden)
                                        .IncludeInMenu(!required)
                                        ;
                                }
                                else if (list.Actions.ContainsKey(c))
                                {
                                    columns.Bound(c).Title(columnTitle)
                                        .ClientTemplate(string.Format("<a href='javascript:{0}(\"#=ItemKey#\");'>#= {1} #</a>", list.Actions[c].Name, c))
                                        .HeaderHtmlAttributes(new { title = columnTitle })
                                        .Width(widths[widthIndex++].Item2)
                                        .Hidden(gridAppearance[c].IsHidden)
                                        .IncludeInMenu(!required)
                                        ;
                                }
                                else
                                {
                                    columns.Bound(c).Title(columnTitle)
                                        .HeaderHtmlAttributes(new { title = columnTitle })
                                        .Width(widths[widthIndex++].Item2)
                                        .Hidden(gridAppearance[c].IsHidden)
                                        .IncludeInMenu(!required)
                                        ;
                                }
                            }
                        }

                        list.Hiddens.ForEach(c => columns
                                                        .Bound(c)
                                                        .Hidden(true)
                                                        .ClientTemplate("<input type='hidden' name='" + c +
                                                                        "' value='#=" + c + "#' />")
                                                        .HtmlAttributes(new {@class = "gridHidden"})
                                                        .IncludeInMenu(false)
                                                        );
                    });
            if (!bCustomToolbar)
            {
                if (bEditable && list.Actions.Count(c => c.Key.StartsWith("@") && c.Value.ActionType == ActionTypes.Create) > 0)
                {
                    var toolBar = CreateToolbar(list.ListKey.ToString(), gridFullName, readOnly || bToolBarBtnsDisable, showEditWindowFuncName);
                    ret = ret.ToolBar(x => x.Template(toolBar));
                }
            }
            if (bFilterable)
            {
                ret = ret.Filterable();
            }
            if (bPageable)
            {
                ret = ret.Pageable(s => s.Messages(InitPagerTranslations));
            }
            if (!bSimpleHeader)
            {
                ret = ret
                    .ColumnMenu(m => m.Filterable(false).Sortable(false).Columns(true).Messages(InitMenuTranslations))
                    .Reorderable(r => r.Columns(true));
            }

            return ret;
        }

        private static string CreateSelectColumnTemplate(string gridName, GridRowSelectionType selectionType, string clientOnSelect)
        {
            var template = new StringBuilder();
            if (selectionType == GridRowSelectionType.Single)
            {
                template.AppendFormat(
                    "<input type='radio' name='{0}' data-item-id='#=ItemKey#' onclick='bvGrid.onRowRadioButtonClick(\"{0}\", this, {1})'/>",
                    gridName, string.IsNullOrEmpty(clientOnSelect) ? "null" : clientOnSelect);
            }
            else if (selectionType == GridRowSelectionType.Multi)
            {
                template.AppendFormat(
                    "<input class='bvCheckbox' type='checkbox' data-item-id='#=ItemKey#' onclick='bvGrid.onCheckboxClick(event, \"{0}\", this, {1})'/><label />",
                    gridName, string.IsNullOrEmpty(clientOnSelect) ? "null" : clientOnSelect);
            }
            else if (selectionType == GridRowSelectionType.None)
            {
                template.Append("<label />");
            }
            return template.ToString();
        }

        private static string CreateEditorsColumnTemplate(ActionMetaItem editAction, ActionMetaItem deleteAction, 
            string listKey, string gridModelFullName, string gridName, string showEditWindowFuncName, string deleteItemFuncName,
            string tooltipEdit, string tooltipDelete)
        {
            var template = new StringBuilder();
            if (editAction != null)
            {
                template.AppendFormat(
                    "<a class='editButton' href='javascript:bvGrid.editRow(\"#=ItemKey#\", \"{0}\", \"{1}\", {2});' {3}></a>",
                    listKey, gridName, showEditWindowFuncName, 
                    string.IsNullOrEmpty(tooltipEdit) ? "" : "title='" + tooltipEdit + "'");
            }
            if (deleteAction != null)
            {
                template.AppendFormat(
                    "<a class='deleteButton' href='javascript:{0}(\"#=ItemKey#\", \"{1}\", \"{2}\", \"{3}\");' {4}></a>",
                    string.IsNullOrEmpty(deleteItemFuncName) ? "bvGrid.deleteRow" : deleteItemFuncName, listKey, gridName, gridModelFullName,
                    string.IsNullOrEmpty(tooltipDelete) ? "" : "title='" + tooltipDelete + "'");
            }
            return template.ToString();
        }

        private static string CreateToolbar(string listKey, string gridName, bool bToolBarBtnsDisable, string showEditWindowFuncName)
        {
            var toolBar = new StringBuilder();
            
                toolBar.AppendFormat(
                    "<input type='button' class='button' value='{0}' data-role='grid-add-button' onclick='bvGrid.addRow(\"{1}\", \"{2}\", {3})' {4}/>",
                    Translator.GetMessageString("strNew"), listKey, gridName, showEditWindowFuncName,
                    bToolBarBtnsDisable ? "disabled='disabled'" : "");
            
            /*var toolBarCustomActions = actions.Where(c => c.Key.StartsWith("@") && c.Value.ActionType == ActionTypes.Action && c.Value.Name != "Create");

            foreach (KeyValuePair<string, ActionMetaItem> action in toolBarCustomActions)
            {
                toolBar.AppendFormat(
                    "<input type='button' class='button' value='{0}' onclick='{1}(\"{2}\", \"{3}\")' {4} {5}/>",
                    action.Value.CaptionId(null, null), action.Value.MethodName, gridName, listKey,
                    bToolBarBtnsDisable || action.Value.OnRow ? "disabled='disabled'" : "", 
                    action.Value.OnRow ? "data-role='grid-on-row-button'" : "data-role='grid-common-button'");
            }*/

            return toolBar.ToString();
        }

        private static List<string> GeoLocationsToBeHidden = new List<string> { "GeoLocationName", "AddressName" };
        private static List<string> SystemDateFields = new List<string> { "datEnteredDate" };

        public static Kendo.Mvc.UI.Fluent.GridBuilder<M> ListFormGrid<M, L>(this HtmlHelper htmlHelper,
            L list,
            IObjectAccessor accessor,
            string controllerName,
            string readActionName = "IndexAjax",
            string cssclass = "lfGrid",
            string gridName = "Grid",
            GridRowSelectionType selectionType = GridRowSelectionType.None,
            string editScript = null,
            string deleteScript = null,
            bool bServerOperation = true,
            int pageSize = 0,
            List<GridModelEditableItem> editableColumns = null,
            string editController = null,
            string editAction = null,
            long editIdKey = 0)
            where M : class, IGridModelItem
            where L : class, IGridModelList
        {
            string accessorFullName = accessor.GetType().FullName;
            var permissions = accessor as IObjectPermissions;
            var gridKey = controllerName + "." + readActionName;
            var gridAppearance = EidssUserContext.User.Options.Grids[gridKey];
            IObjectMeta meta = list.ObjectMeta;

            Action<Kendo.Mvc.UI.Fluent.CrudOperationBuilder> readAction
                 = read => read.Action(readActionName, controllerName).Data("function () { return searchPanel.getFilter!=null ? searchPanel.getFilter('" + gridName + "') : {}; }");

            var widths = CalculateColumnSizes(selectionType, true, null, list, editableColumns, gridAppearance);
            int widthIndex = 0;

            var ret = htmlHelper.Kendo().Grid<M>()
                .Name(gridName)
                .Sortable(s => s.AllowUnsort(false).SortMode(GridSortMode.SingleColumn))
                .Scrollable(s => s.Enabled(true))
                .HtmlAttributes(new { @class = cssclass })
                .Resizable(r => r.Columns(true))
                .Pageable(s => s.Messages(InitPagerTranslations))
                .Selectable(s => s.Enabled(false))
                .Events(events => events
                    .Edit("function(e) { bvGrid.onEdit(e); }")
                    .ColumnMenuInit("function(e) { bvGrid.onMenuInit(e, '" + gridKey + "', " + (editableColumns == null ? "false" : "true") + "); }")
                    .ColumnHide("function(e) { bvGrid.onColumnHide(e, '" + gridKey + "'); }")
                    .ColumnShow("function(e) { bvGrid.onColumnShow(e, '" + gridKey + "'); }")
                    .ColumnReorder("function(e) { bvGrid.onColumnReorder(e, '" + gridKey + "'); }")
                    .ColumnResize("function(e) { bvGrid.onColumnResize(e, '" + gridKey + "'); }")
                    )
                .ColumnMenu(m => m.Filterable(false).Sortable(false).Columns(true).Messages(InitMenuTranslations))
                .Reorderable(r => r.Columns(true))
                .Editable(s => { if (editableColumns == null) s.Enabled(false); else s.Mode(GridEditMode.InCell); })
                .DataSource(source => 
                    {
                        var ajax = source.Ajax();
                        ajax
                        .Read(readAction)
                        .ServerOperation(bServerOperation)
                        .Events(c => c.RequestEnd("function (e) { searchPanel.requestEnd(e); }"))
                        .PageSize(pageSize == 0 ? EidssUserContext.User.Options.Prefs.ListGridPageSize : pageSize)
                        .Model(model =>
                            {
                                model.Id(p => p.ItemKey);
                                if (editableColumns != null)
                                {
                                    foreach (var c in list.Columns)
                                    {
                                        var edit = editableColumns.FirstOrDefault(i => i.Name == c);
                                        if (edit != null)
                                        {
                                            model.Field(edit.Name, edit.Type).Editable(true);
                                        }
                                        else
                                        {
                                            model.Field<string>(c).Editable(false);
                                        }
                                    }
                                    model.Field<string>("ItemKey").Editable(false);
                                }
                            });
                        if (editableColumns != null)
                        {
                            ajax
                                .AutoSync(true)
                                .Events(e => e
                                    //.Change("function(e) {bvGrid.onChange(e);}")
                                    //.Push("function(e) {bvGrid.onPush(e);}")
                                    .RequestEnd("function(e) { searchPanel.requestEnd(e); bvGrid.onRequestEnd(e); }")
                                    //.RequestStart("function(e) {bvGrid.onRequestStart(e);}")
                                    //.Sync("function(e) {bvGrid.onSync(e);}")
                                    .Error("function(e, status) {bvGrid.onError(e, status);}")
                                    )
                                .Update(editAction, editController, new { key = editIdKey });
                        }
                    })
                .Columns(columns =>
                {
                    if (selectionType != GridRowSelectionType.None)
                    {
                        string columnTemplate = CreateSelectColumnTemplate(gridName, selectionType, null);
                        columns.Bound("ItemKey")
                               .ClientTemplate(columnTemplate)
                               .Title(string.Empty)
                               .Sortable(false)
                               .HeaderHtmlAttributes(new { title = BvMessages.Get("strSelectAll_Id"), @class = "checkColumn" })
                               .HtmlAttributes(new { @class = "checkColumn" })
                               .Width(widths[widthIndex++].Item2)
                               .IncludeInMenu(false)
                               ;
                    }

                    //if (permissions.CanDelete && permissions.CanUpdate)
                    //{

                    bool bSomeFieldsAreHidden = false;
                    foreach (var columnName in widths.Where(i => !String.IsNullOrEmpty(i.Item1)).Select(i => i.Item1))
                    {
                        bSomeFieldsAreHidden |= gridAppearance[columnName].IsHidden;
                    }


                    string hrefEdit = (editScript == null)
                        ? string.Format("javascript:actions.edit(\"/{0}/Details?id=#=ItemKey#\");", controllerName)
                        : string.Format("javascript:{0}(\"#=ItemKey#\");", editScript);
                    string hrefDelete = (deleteScript == null)
                        ? string.Format("javascript:actions.deleteItemFromList(\"#=ItemKey#\", \"{0}\");", accessorFullName)
                        : string.Format("javascript:{0}(\"#=ItemKey#\");", deleteScript);
                    columns.Bound("ItemKey")
                           .ClientTemplate(
                               string.Format("<a class='editButton' href='{0}'></a>", hrefEdit)
                               + (permissions.CanDelete
                                    ? string.Format("<a class='deleteButton' href='{0}'></a>", hrefDelete)
                                    : "")
                               )
                           .Sortable(false)
                           .Title(string.Empty)
                           .HeaderHtmlAttributes(new { @class = "editColumn" })
                           .HeaderTemplate("<div class='k-link' style='padding-bottom: 0.3em; padding-top: 0.6em; text-overflow: clip; '><img id='idSomeFieldsAreHidden' src='/Content/Images/warning12x12.png' title='" + Translator.GetBvMessageString("msgSomeFieldsAreHidden") + "' style='display: " + (bSomeFieldsAreHidden ? "inline;" : "none;") + "' /></div>")
                           .HtmlAttributes(new { @class = "editColumn" })
                           .Width(widths[widthIndex++].Item2)
                           .IncludeInMenu(false)
                           ;
                    //}

                    foreach (var columnName in widths.Where(i => !String.IsNullOrEmpty(i.Item1)).Select(i => i.Item1))
                    {
                        string columnTitle = Translator.GetString(Translator.GetString(list.Labels.ContainsKey(columnName) ? list.Labels[columnName] : columnName));
                        //SizeF columnTitleSize = graphics.MeasureString(columnTitle, columnTitleFont);
                        //int columnTitleWidth = Convert.ToInt32(columnTitleSize.Width) + 20;
                        bool required = meta.GridMeta.FirstOrDefault(i => i.Name == columnName, i => i.Required);

                        if (list.IsHiddenPersonalData(columnName) && !GeoLocationsToBeHidden.Contains(columnName))
                        {
                            columns.Bound(columnName).ClientTemplate("*******").Title(columnTitle)
                                .HeaderHtmlAttributes(new { title = columnTitle })
                                .Width(widths[widthIndex++].Item2)
                                .Hidden(gridAppearance[columnName].IsHidden)
                                .IncludeInMenu(!required)
                                ;
                        }
                        else
                        {
                            if (columnName.StartsWith("dat"))
                            {
                                if (editableColumns != null && editableColumns.Any(i => i.Name == columnName))
                                {
                                    columns.Bound(columnName).Title(columnTitle)
                                       .HeaderHtmlAttributes(new { title = columnTitle })
                                       .Format("{0:d}")
                                       .Width(widths[widthIndex++].Item2)
                                       .Hidden(gridAppearance[columnName].IsHidden)
                                       .IncludeInMenu(!required)
                                       ;
                                }
                                else
                                {
                                    if (editableColumns != null)
                                    {
                                        columns
                                            .Bound(columnName)
                                            .Format("{0:d}")//.Format("{0:G}")
                                            .Title(columnTitle)
                                            .HeaderHtmlAttributes(new { title = columnTitle })
                                            .Width(widths[widthIndex++].Item2)
                                            .Hidden(gridAppearance[columnName].IsHidden)
                                            .IncludeInMenu(!required)
                                            ;
                                    }
                                    else
                                    {
                                        columns
                                            .Bound(columnName)
                                            .ClientTemplate(String.Format("#={0} == null ? \"\" : {0}._tostring#", columnName))
                                            .Title(columnTitle)
                                            .HeaderHtmlAttributes(new { title = columnTitle })
                                            .Width(widths[widthIndex++].Item2)
                                            .Hidden(gridAppearance[columnName].IsHidden)
                                            .IncludeInMenu(!required)
                                            ;
                                    }
                                }
                            }
                            else if (list.Actions.ContainsKey(columnName))
                            {
                                columns
                                    .Bound(columnName)
                                    .ClientTemplate(string.Format("<a href='javascript:{0}(\"#=ItemKey#\");'>#= {1} #</a>", list.Actions[columnName].Name, columnName))
                                    .Title(columnTitle)
                                    .HeaderHtmlAttributes(new { title = columnTitle })
                                    .Width(widths[widthIndex++].Item2)
                                    .Hidden(gridAppearance[columnName].IsHidden)
                                    .IncludeInMenu(!required)
                                    ;
                            }
                            else
                            {
                                if (editableColumns != null && editableColumns.Any(i => i.Name == columnName && !string.IsNullOrEmpty(i.ClientTemplate)))
                                {
                                    columns.Bound(columnName).Title(columnTitle)
                                        .ClientTemplate(editableColumns.FirstOrDefault(i => i.Name == columnName && !string.IsNullOrEmpty(i.ClientTemplate), i => i.ClientTemplate))
                                        .HeaderHtmlAttributes(new { title = columnTitle })
                                        .Width(widths[widthIndex++].Item2)
                                        .Hidden(gridAppearance[columnName].IsHidden)
                                        .IncludeInMenu(!required)
                                       ;
                                }
                                else
                                {
                                    columns.Bound(columnName).Title(columnTitle)
                                        .HeaderHtmlAttributes(new { title = columnTitle })
                                        .Width(widths[widthIndex++].Item2)
                                        .Hidden(gridAppearance[columnName].IsHidden)
                                        .IncludeInMenu(!required)
                                        ;
                                }
                            }
                        }
                    }
                    columns.Bound("ItemKey").Hidden(true).Sortable(false).HtmlAttributes(new { @class = "gridId" }).IncludeInMenu(false);
                })
                .AutoBind(true);

            return ret;
        }

        public static Kendo.Mvc.UI.Fluent.GridBuilder<M> DetailsPopupSearchGrid<M, L>
            (this HtmlHelper htmlHelper,
             L list,
             string controllerName,
             string readActionName = "IndexAjax",
             string gridName = "Grid",
             string cssclass = "detailsGrid popupSearchGrid",
             GridRowSelectionType selectionType = GridRowSelectionType.None,
            string getFilterFuncName = "searchPanel.getFilter", bool bPageable = true)
            where M : class, IGridModelItem
            where L : class, IGridModelList
        {
            Action<Kendo.Mvc.UI.Fluent.CrudOperationBuilder> readAction = 
                read => read.Action(readActionName, controllerName)
                    .Data(string.Format("function () {{ $(\"#bFirstSearchClick\").val(\"1\"); return {0}!=null ? {0}('{1}') : {{}}; }}", getFilterFuncName, gridName));

            var gridKey = controllerName + "." + readActionName;
            var gridAppearance = EidssUserContext.User.Options.Grids[gridKey];
            IObjectMeta meta = list.ObjectMeta;

            var widths = CalculateColumnSizes(selectionType, false, null, list, null, gridAppearance);
            int widthIndex = 0;

            var ret = htmlHelper.Kendo().Grid<M>()
                                .Name(gridName)
                                .Scrollable(s => s.Enabled(true))
                                .Selectable(s => s.Enabled(false))
                                .Sortable(s => s.AllowUnsort(false).SortMode(GridSortMode.SingleColumn))
                                .HtmlAttributes(new {@class = cssclass})
                                .Resizable(r => r.Columns(true)).AutoBind(true)
                                .Events(events => events
                                    .DataBound("function(e) { bvGrid.onDetailsPopupGridDataBound(e); }")
                                    .DataBinding("function(e) { bvGrid.onDetailsPopupGridDataBinding(e); }")
                                    .ColumnMenuInit("function(e) { bvGrid.onMenuInit(e, '" + gridKey + "', true); }")
                                    .ColumnHide("function(e) { bvGrid.onColumnHide(e, '" + gridKey + "'); }")
                                    .ColumnShow("function(e) { bvGrid.onColumnShow(e, '" + gridKey + "'); }")
                                    .ColumnReorder("function(e) { bvGrid.onColumnReorder(e, '" + gridKey + "'); }")
                                    .ColumnResize("function(e) { bvGrid.onColumnResize(e, '" + gridKey + "'); }")
                                    )
                                .ColumnMenu(m => m.Filterable(false).Sortable(false).Columns(true).Messages(InitMenuTranslations))
                                .Reorderable(r => r.Columns(true))
                                .DataSource(source =>
                                    {
                                        var ajax = source.Ajax();
                                        if (bPageable)
                                        {
                                            ajax = ajax.PageSize(EidssUserContext.User.Options.Prefs.PopupGridPageSize);
                                        }
                                        ajax
                                            .Read(readAction)
                                            .Events(c => c.RequestEnd("function (e) { searchPanel.requestEnd(e); }"))
                                            .Model(model => model.Id(p => p.ItemKey));
                                    })
                                .Columns(columns =>
                                    {
                                        bool bSomeFieldsAreHidden = false;
                                        foreach (var columnName in widths.Where(i => !String.IsNullOrEmpty(i.Item1)).Select(i => i.Item1))
                                        {
                                            bSomeFieldsAreHidden |= gridAppearance[columnName].IsHidden;
                                        }

                                        if (selectionType != GridRowSelectionType.None)
                                        {
                                            string columnTemplate = CreateSelectColumnTemplate(gridName, selectionType, null);
                                            columns.Bound("ItemKey")
                                                   .ClientTemplate(columnTemplate)
                                                   .Title(string.Empty)
                                                   .Sortable(false)
                                                   .HeaderHtmlAttributes(new {@class = "checkColumn"})
                                                   .HeaderTemplate("<div class='k-link' style='padding-bottom: 0.3em; padding-top: 0.6em; text-overflow: clip; '><img id='idSomeFieldsAreHidden' src='/Content/Images/warning12x12.png' title='" + Translator.GetBvMessageString("msgSomeFieldsAreHidden") + "' style='display: " + (bSomeFieldsAreHidden ? "inline;" : "none;") + "' /></div>")
                                                   .HtmlAttributes(new { @class = "checkColumn" })
                                                   .Width(widths[widthIndex++].Item2)
                                                   .IncludeInMenu(false)
                                                   ;
                                        }

                                        //foreach (var columnName in list.Columns)
                                        foreach (var columnName in widths.Where(i => !String.IsNullOrEmpty(i.Item1)).Select(i => i.Item1))
                                        {
                                            bool required = meta.GridMeta.FirstOrDefault(i => i.Name == columnName, i => i.Required);
                                            string columnTitle =
                                                Translator.GetString(list.Labels.ContainsKey(columnName)
                                                                         ? list.Labels[columnName]
                                                                         : columnName);

                                            //SizeF columnTitleSize = graphics.MeasureString(columnTitle, columnTitleFont);
                                            //int columnTitleWidth = Convert.ToInt32(columnTitleSize.Width) + 20;

                                            if (list.IsHiddenPersonalData(columnName) && !GeoLocationsToBeHidden.Contains(columnName))
                                            {
                                                columns.Bound(columnName).ClientTemplate("*******").Title(columnTitle)
                                                       .HeaderHtmlAttributes(new {title = columnTitle})
                                                       .Width(widths[widthIndex++].Item2)
                                                       .Hidden(gridAppearance[columnName].IsHidden)
                                                       .IncludeInMenu(!required)
                                                       ;
                                            }
                                            else
                                            {
                                                if (columnName.StartsWith("dat"))
                                                {
                                                    /*if (!SystemDateFields.Contains(columnName))
                                                    {
                                                        columns.Bound(columnName).Title(columnTitle)
                                                            .HeaderHtmlAttributes(new { title = columnTitle })
                                                            .ClientTemplate(String.Format("#=bvGrid.fixDateTime({0}, '{1}')#", columnName, "{0:d}"))
                                                            .Width(widths[widthIndex++])
                                                            ;
                                                    }
                                                    else
                                                    {*/
                                                    columns.Bound(columnName).Title(columnTitle)
                                                           .HeaderHtmlAttributes(new {title = columnTitle})
                                                        //.Format("{0:d}")
                                                        //.ClientTemplate(String.Format("#=bvGrid.fixDateTime({0}, '{1}')#", columnName, "d"))
                                                           .ClientTemplate(
                                                               String.Format("#={0} == null ? \"\" : {0}._tostring#",
                                                                             columnName))
                                                           .Width(widths[widthIndex++].Item2)
                                                           .Hidden(gridAppearance[columnName].IsHidden)
                                                           .IncludeInMenu(!required)
                                                           ;
                                                    /*}*/
                                                }
                                                else
                                                {
                                                    columns
                                                        .Bound(columnName)
                                                        .Title(columnTitle)
                                                        .HeaderHtmlAttributes(new {title = columnTitle})
                                                        .Width(widths[widthIndex++].Item2)
                                                        .Hidden(gridAppearance[columnName].IsHidden)
                                                        .IncludeInMenu(!required)
                                                        ;
                                                }
                                            }
                                        }
                                    });
            if (bPageable)
            {
                ret = ret.Pageable(s => s.Messages(InitPagerTranslations));
            }

            return ret;
        }
        private static void InitPagerTranslations(PageableMessagesBuilder builder)
        {
            builder.Empty(Translator.GetMessageString("kendoGridEmpty"));
            builder.Display(Translator.GetMessageString("kendoGridDisplay"));
            builder.ItemsPerPage(Translator.GetMessageString("kendoGridItemsPerPage"));
            builder.Page(Translator.GetMessageString("kendoGridPage"));
            builder.Of(Translator.GetMessageString("kendoGridOf"));
            builder.Refresh(Translator.GetMessageString("kendoGridRefresh"));
            builder.First(Translator.GetMessageString("kendoGridFirst"));
            builder.Last(Translator.GetMessageString("kendoGridLast"));
            builder.Next(Translator.GetMessageString("kendoGridNext"));
            builder.Previous(Translator.GetMessageString("kendoGridPrevious"));
            builder.MorePages(Translator.GetMessageString("kendoGridMorePages"));
        }
        private static void InitMenuTranslations(ColumnMenuMessagesBuilder builder)
        {
            builder.Columns(Translator.GetMessageString("kendoMenuColumns"));
        }
        #endregion

        #region Address extenders

        public static MvcHtmlString Address(this HtmlHelper htmlHelper, long root, eidss.model.Schema.Address address, bool isCountryFieldVisible = false,
            bool isForeignAddressFieldVisible = false, bool needFillRegionOnCountryChanged = false, bool isCoordinatesFieldsVisible = false)
        {
            var args = new RouteValueDictionary { { "root", root }, { "address", address }, { "isCountryFieldVisible", isCountryFieldVisible }, 
                { "isForeignAddressFieldVisible", isForeignAddressFieldVisible }, { "needFillRegionOnCountryChanged", needFillRegionOnCountryChanged },
                {"isCoordinatesFieldsVisible", isCoordinatesFieldsVisible}};
            return htmlHelper.Action("Address", "Address", args);
        }

        public static MvcHtmlString ReadOnlyAddress(this HtmlHelper htmlHelper, long root, eidss.model.Schema.Address address)
        {
            var args = new RouteValueDictionary { { "root", root }, { "address", address } };
            return htmlHelper.Action("ReadOnlyAddress", "Address", args);
        }

       /* public static MvcHtmlString InlineAddressPicker(this HtmlHelper htmlHelper, long root, eidss.model.Schema.Address address)
        {
            var args = new RouteValueDictionary { { "root", root }, { "address", address } };
            return htmlHelper.Action("InlineAddressPicker", "Address", args);
        }*/

        public static MvcHtmlString AddressInTwoColumns(this HtmlHelper htmlHelper, long root, eidss.model.Schema.Address address, bool isCountryFieldVisible = false,
            bool isForeignAddressFieldVisible = false, bool needFillRegionOnCountryChanged = false, bool isCoordinatesFieldsVisible = false)
        {
            var args = new RouteValueDictionary { { "root", root }, { "address", address }, { "isCountryFieldVisible", isCountryFieldVisible }, 
                { "isForeignAddressFieldVisible", isForeignAddressFieldVisible }, { "needFillRegionOnCountryChanged", needFillRegionOnCountryChanged },
                {"isCoordinatesFieldsVisible", isCoordinatesFieldsVisible}};
            return htmlHelper.Action("AddressInTwoColumns", "Address", args);
        }

        public static MvcHtmlString AddressWithCountry(this HtmlHelper htmlHelper, long root, eidss.model.Schema.Address address)
        {
            var args = new RouteValueDictionary { { "root", root }, { "address", address } };
            return htmlHelper.Action("AddressWithCountry", "Address", args);
        }

        #endregion

        #region ActionPanel extenders

        public static void RenderActionPanel(this HtmlHelper htmlHelper, IObjectAccessor accessor, IObject obj, ActionsPanelType panelType)
        {
            if (accessor != null)
                htmlHelper.RenderAction("Index", "ActionPanel", 
                    new RouteValueDictionary { { "accessor", accessor }, { "obj", obj }, { "panelType", panelType } });
        }
        #endregion
    }
}
