﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using bv.common.Core;
using bv.common.Enums;
using bv.common.Resources;
using bv.common.Resources.Images;
using bv.common.Resources.TranslationTool;
using bv.model.BLToolkit;
using bv.model.Model.Core;
using bv.winclient.BasePanel;
using bv.winclient.Core;
using bv.winclient.Core.TranslationTool;
using bv.winclient.Layout;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using System.ComponentModel;
using bv.common.Diagnostics;

namespace bv.winclient.ActionPanel
{
    public partial class BaseActionPanel : BvXtraUserControl
    {
        protected readonly ActionLocker m_ActionLocker = new ActionLocker();
        public BaseActionPanel()
        {
            InitializeComponent();

            m_Buttons = new List<Control>();

        }

        /// <summary>
        /// Ссылка на Layout, на котором лежит эта панель
        /// </summary>
        public LayoutEmpty ParentLayout { get; internal set; }

        private IBaseListPanel m_BaseListPanel;

        private List<ActionMetaItem> m_AllActions;

        /// <summary>
        /// 
        /// </summary>
        protected IBaseListPanel BaseGridPanel
        {
            get
            {
                if (m_BaseListPanel == null)
                {
                    if (ParentLayout != null) m_BaseListPanel = ParentLayout.ParentBasePanel as IBaseListPanel;
                }
                return m_BaseListPanel;
            }
        }

        private IBasePanel m_BasePanel;

        /// <summary>
        /// 
        /// </summary>
        protected IBasePanel BasePanel
        {
            get
            {
                if (m_BasePanel == null)
                {
                    if (ParentLayout != null) m_BasePanel = ParentLayout.ParentBasePanel;
                }
                return m_BasePanel;
            }
        }

        /// <summary>
        /// Визуализация действий на панели
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="panelType"></param>
        public void AddActionsRoutines(List<ActionMetaItem> actions, ActionsPanelType panelType)
        {
            m_AllActions = actions;
            var avaliableActions = actions.Where(c => c.PanelType.Equals(panelType)).ToList();
            ////var permissions = BusinessObject != null ? BusinessObject.GetAccessor() as IObjectPermissions : null;
            foreach (var action in avaliableActions)
            {
                //проверим, есть ли у текущего пользователя права на это действие
                //если нет, то не рендерим это действие
                //perm!!var showAction = action.CheckPermission(permissions, BusinessObject != null && BusinessObject.ReadOnly);
                //perm!! ParentLayout.ParentBasePanel.CheckActionPermission(action, ref showAction);
                //perm!!if (!showAction) continue;
                /*perm!!
                if (permissions != null)
                {
                    switch(action.ActionType)
                    {
                        case ActionTypes.Edit:
                            action.ReadOnly = !permissions.CanUpdate || ParentLayout.ParentBasePanel.ReadOnly;
                            break;
                        case ActionTypes.View:
                            action.ReadOnly = permissions.CanUpdate;
                            break;
                        case ActionTypes.Delete:
                            action.ReadOnly = !permissions.CanDelete;
                            break;
                        case ActionTypes.Create:
                            action.ReadOnly = !permissions.CanInsert;
                            break;
                    }
                }*/
                if (action.IsVisible(BusinessObject, Permissions) && (action.AppType == ActionsAppType.All || action.AppType == ActionsAppType.Win))
                    AddAction(action);
            }
            RecalculateActionsPositions();
            if (BaseGridPanel != null)
            {
                BaseGridPanel.RefreshFocusedItem();
            }

        }

        /// <summary>
        /// Вычисляем положение кнопок на панели
        /// </summary>
        protected virtual void RecalculateActionsPositions()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected IObject BusinessObject { get; set; }

        protected IObjectPermissions Permissions { get { return BusinessObject != null ? BusinessObject.GetAccessor() as IObjectPermissions : null; } }

        ///// <summary>
        ///// Отыскивает среди кнопок действий ту, чей тип совпадает с переданным (находит первую)
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //private Control GetButtonAction(ActionTypes type)
        //{
        //    SimpleButton result = null;
        //    foreach (var button in m_Buttons)
        //    {
        //        var action = button.Tag as ActionMetaItem;
        //        if (action == null) continue;
        //        if (action.ActionType.Equals(type))
        //        {
        //            result = button;
        //            break;
        //        }
        //    }
        //    return result;
        //}

        protected readonly List<Control> m_Buttons;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnButtonClick(object sender, EventArgs e)
        {
            var button = sender as SimpleButton;
            if (button == null) return;
            var action = button.Tag as ActionMetaItem;
            if (action != null)
            {
                RunAction(action, null);
                //если есть связанное действие, то выполняем его
                //if (action.ActionLinked != null) RunAction(action.ActionLinked, null);
            }
        }

        public delegate void ActionMetaItemDelegate(ActionMetaItem result);


        /// <summary>
        /// Последнее вызванное действие на любой Action Panel
        /// </summary>
        public event ActionMetaItemDelegate OnLastExecutedActionChanged;

        public delegate void BeforeActionExecutingDelegate(IBasePanel panel, ActionMetaItem action, IObject bo, ref bool cancel);
        public event BeforeActionExecutingDelegate OnBeforeActionExecuting;
        public delegate void AfterActionExecutedDelegate(IBasePanel panel, ActionMetaItem action, IObject bo);
        public event AfterActionExecutedDelegate OnAfterActionExecuted;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="externalParams"> </param>
        internal virtual void RunAction(ActionMetaItem action, List<object> externalParams)
        {
            WaitDialogType waitType;
            switch (action.ActionType)
            {
                //case ActionTypes.Create:
                //case ActionTypes.Edit:
                //case ActionTypes.View:
                //    waitType = WaitDialogType.FormLoading;
                //    break;
                default:
                    waitType = WaitDialogType.None;
                    break;
            }
            if (!m_ActionLocker.Lock(waitType)) return;

            try
            {
                var needCancel = false;
                if (OnBeforeActionExecuting != null)
                {
                    /*
                    IObject bo;
                    if (BaseGridPanel != null && BaseGridPanel.FocusedItem != null) 
                        bo = BaseGridPanel.FocusedItem;
                    else 
                        bo = BusinessObject;
                    */
                    OnBeforeActionExecuting(ParentLayout != null ? ParentLayout.ParentBasePanel : null
                                            , action
                                            , BusinessObject
                                            , ref needCancel);
                }
                if (needCancel) return;

                //определим набор параметров, которые могут быть переданы от родительской панели
                //сначала получаем указатель на панель, которую обслуживает эта панель
                var parentBasePanel = ParentLayout != null ? ParentLayout.ParentBasePanel.ParentBasePanel : null;
                var parameters = externalParams;
                if (parentBasePanel != null && externalParams == null) parameters = parentBasePanel.GetParamsAction(BusinessObject);

                #region Выполнение действий

                var currentBo = BusinessObject;

                switch (action.ActionType)
                {
                    case ActionTypes.Action:
                        // For Grid panels only we should pass Key of Selected Item  as parameter
                        using (var manager = CreateDbManagerProxy())
                        {
                            bool bSuccess;
                            if (externalParams != null)
                            {
                                bSuccess = action.RunAction(manager, BusinessObject, externalParams).result;
                            }
                            else if ((BaseGridPanel != null))
                            {
                                BaseGridPanel.RefreshFocusedItem();
                                currentBo = BaseGridPanel.FocusedItem;

                                var prs = CreatePanelGridParameters(BaseGridPanel);
                                prs.AddRange(action.Parameters);
                                bSuccess = action.RunAction(manager, currentBo, prs).result;
                            }
                            else
                            {
                                bSuccess = action.RunAction(manager, BusinessObject).result;
                            }
                            if (bSuccess)
                            {
                                if (action.IsNeedClose) ActionCloseForm(action.Name == "OK" ? DialogResult.OK : DialogResult.Cancel);
                                if (!String.IsNullOrEmpty(action.RelatedLists))
                                {
                                    var names = action.RelatedLists.Split(',');
                                    foreach (var name in names)
                                        BaseFormManager.RefreshList(name);
                                }
                            }

                        }
                        break;

                    case ActionTypes.Create:
                        if (BaseGridPanel != null)
                        {
                            BaseGridPanel.Edit(null, parameters, action.ActionType, false);
                            currentBo = BaseGridPanel.FocusedItem;
                        }
                        else if ((ParentLayout != null) && (ParentLayout.ParentBasePanel != null))
                        {
                            //TODO необходимо проверить этот код
                            var saveAction =
                                m_AllActions.SingleOrDefault(c => c.ActionType == ActionTypes.Save) ??
                                m_AllActions.SingleOrDefault(c => c.ActionType == ActionTypes.Ok);
                            if (saveAction != null)
                            {
                                if (ActionSave(saveAction, true))
                                {
                                    using (var manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                                    {
                                        var bo = action.RunAction(manager, null as IObject, parameters).obj;
                                        if (bo != null)
                                        {
                                            currentBo = bo;
                                            ParentLayout.ParentBasePanel.BusinessObject = bo;
                                            ParentLayout.BusinessObject = bo;
                                            BusinessObject = bo;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ActionTypes.Ok:
                        //проверить наличие изменений + сохранить + закрыть форму
                        var success = true;
                        if (!BusinessObject.ReadOnly) success = ActionSave(action, false);
                        if (success) ActionCloseForm(DialogResult.OK);
                        break;
                    case ActionTypes.Cancel:
                        //проверить наличие изменений + не сохранять + закрыть форму
                        if (ActionCanCancel()) ActionCloseForm();
                        break;
                    case ActionTypes.Close:
                        //закрыть форму
                        var successClose = true;
                        if (BusinessObject != null)
                        {
                            using (var manager = CreateDbManagerProxy())
                            {
                                successClose = action.RunAction(manager, BusinessObject).result;
                            }
                        }
                        if (successClose) ActionCloseForm();
                        break;
                    case ActionTypes.Delete:
                        //применяется только для списочных форм
                        //проверить наличие активного элемента + сохранить + обновление
                        if ((BaseGridPanel != null)
                            && (BaseGridPanel.FocusedItem != null)
                            && (ParentLayout != null))
                        {
                            #region Для списочных форм

                            /*if (BaseGridPanel.FocusedItem.HasChanges && !BaseGridPanel.FocusedItem.IsNew)
                            {
                                ErrorForm.ShowWarning("msgCantDelete", "Can't delete object");
                            }
                            else*/
                            if (WinUtils.ConfirmDelete())
                            {
                                BaseGridPanel.RefreshFocusedItem();
                                var itemsToDelete = BaseGridPanel.SelectedItems.Count > 0
                                                        ? BaseGridPanel.SelectedItems
                                                        : new List<IObject> { BaseGridPanel.FocusedItem };
                                currentBo = BaseGridPanel.FocusedItem;
                                try
                                {
                                    var appForm = (parentBasePanel ?? ParentLayout.ParentBasePanel) as IApplicationForm;
                                    if (
                                        BaseFormManager.FindFormByID(appForm, currentBo.Key) != null)
                                    {
                                        ErrorForm.ShowWarning("msgCantDelete", "Can't delete object");
                                        break;
                                    }

                                    using (var manager = CreateDbManagerProxy())
                                    {
                                        foreach (var item in itemsToDelete)
                                        {
                                            item.Validation += GridPanelItem_DeleteValidation;
                                            if (action.RunAction(manager, item).result)
                                            {
                                                if (!(BaseGridPanel is IChildListPanel))
                                                {
                                                    BaseFormManager.RefreshList(BusinessObject.GetType().Name);
                                                    //BaseGridPanel.Delete(item.Key);
                                                }
                                                else
                                                {
                                                    BaseGridPanel.Grid.GridView.BeginDataUpdate();
                                                    var filter =
                                                        BaseGridPanel.Grid.GridView.ActiveFilter.NonColumnFilter;
                                                    BaseGridPanel.Grid.GridView.ActiveFilter.NonColumnFilter =
                                                        String.Empty;
                                                    BaseGridPanel.Grid.GridView.ActiveFilter.NonColumnFilter = filter;
                                                    BaseGridPanel.Grid.GridView.EndDataUpdate();
                                                }
                                                if (!string.IsNullOrEmpty(action.RelatedLists))
                                                {
                                                    var names = action.RelatedLists.Split(',');
                                                    foreach (var name in names)
                                                        if (name != BusinessObject.GetType().Name)
                                                            BaseFormManager.RefreshList(name);

                                                }

                                            }

                                        }
                                    }
                                }
                                finally
                                {
                                    foreach (var item in itemsToDelete)
                                    {
                                        item.Validation -= GridPanelItem_DeleteValidation;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if
                            (
                            (ParentLayout is LayoutSimple)
                            && (BusinessObject != null)
                            )
                        {
                            if (!BusinessObject.IsNew && BusinessObject.HasChanges)
                            {
                                ErrorForm.ShowWarning("msgCantDelete", "Can't delete object");
                            }
                            else if (WinUtils.ConfirmDelete())
                            {
                                currentBo = BusinessObject;
                                //if (
                                //    BaseFormManager.FindFormByID(ParentLayout.ParentBasePanel as IApplicationForm,
                                //                                 currentBo.Key) != null)
                                //{
                                //    ErrorForm.ShowWarning("msgCantDelete", "Can't delete object");
                                //    break;
                                //}

                                using (var manager = CreateDbManagerProxy())
                                {
                                    if (action.RunAction(manager, BusinessObject).result) ActionCloseForm();
                                }
                            }
                        }

                        break;
                    case ActionTypes.Refresh:

                        //применяется только для списочных форм
                        //получить данные из БД
                        if (BaseGridPanel != null) BaseGridPanel.Refresh();
                        break;
                    case ActionTypes.View:
                    case ActionTypes.Edit:

                        //применяется только для списочных форм
                        //проверить наличие активного элемента + открыть форму редактирования (сохранения внутри неё) + обновить
                        if (BaseGridPanel != null)
                        {
                            var readOnly = action.ActionType == ActionTypes.View || action.IsReadOnly(BusinessObject, Permissions);
                            var focusedItem = BaseGridPanel.FocusedItem;
                            if (BaseGridPanel.EnableMultiEdit && BaseGridPanel.SelectedItems.Count > 1)
                            {
                                var keys = BaseGridPanel.SelectedItems.Select(bo => bo.Key).ToList();
                                BaseGridPanel.Edit(keys, CreatePanelGridParameters(BaseGridPanel), action.ActionType, readOnly);
                            }
                            else if ((focusedItem != null) && (focusedItem.Key != null))
                            {
                                currentBo = focusedItem;
                                BaseGridPanel.Edit(focusedItem.Key, CreatePanelGridParameters(BaseGridPanel), action.ActionType, readOnly);
                            }
                        }
                        break;
                    case ActionTypes.Save:
                        //проверить наличие изменений + сохранить
                        ActionSave(action, true);
                        break;
                    case ActionTypes.Report:

                        if ((ParentLayout != null) && (ParentLayout.ParentBasePanel != null))
                        {
                            ParentLayout.ParentBasePanel.ShowReport();
                        }
                        break;
                    case ActionTypes.Select:
                        if ((BaseGridPanel != null) && (BaseGridPanel.FocusedItem != null))
                        {
                            ActionCloseForm(DialogResult.OK);
                        }
                        break;
                    case ActionTypes.SelectAll:
                        if (BaseGridPanel != null)
                        {
                            BaseGridPanel.SelectAll();
                            if (BaseGridPanel.SelectedItems != null && BaseGridPanel.SelectedItems.Count > 0)
                                ActionCloseForm(DialogResult.OK);
                        }
                        break;
                }

                #endregion

                if (OnLastExecutedActionChanged != null) OnLastExecutedActionChanged(action);

                RaiseAfterActionExecuted(action, currentBo);
            }
            catch (PermissionException exc)
            {
                ErrorForm.ShowWarning("msgNoExecutePermission");
                Dbg.Debug("Permission exception for {0}.{1}, exeption:{2}", exc.ObjectName, exc.ActionName, exc);
            }
            catch (ParamsCountException exc)
            {
                ErrorForm.ShowError(StandardError.ParamsCountError, exc);
            }
            catch (DbModelTimeoutException)
            {
                ErrorForm.ShowWarning("msgTimeoutList", "Cannot retrieve records from database because of the timeout. Please change search criteria and try again");
            }
            catch (DbModelRaiserrorException ex2)
            {
                if (string.IsNullOrEmpty(ex2.MessageId))
                    ErrorForm.ShowError(ex2.Message, ex2);
                else
                {
                    var args = ex2.MessageId.Split(new[] { ' ' });
                    if (args.Count() > 1)
                    {
                        var pars = new object[args.Count() - 1];
                        for (int i = 1; i < args.Count(); i++)
                            pars[i - 1] = args[i];
                        ErrorForm.ShowError(args[0], args[0], pars);
                    }
                    else
                        ErrorForm.ShowError(ex2.MessageId, ex2.Message, ex2);
                }
            }
            catch (DbModelException ex1)
            {
                if (string.IsNullOrEmpty(ex1.MessageId))
                    ErrorForm.ShowError(ex1.Message, ex1);
                else
                    ErrorForm.ShowError(ex1.MessageId, ex1.Message, ex1);
            }
            catch (BvModelException exc)
            {
                ErrorForm.ShowError(StandardError.UnprocessedError, exc.InnerException ?? exc);
            }
            catch (Exception exc)
            {
                ErrorForm.ShowError(StandardError.UnprocessedError, exc);
            }
            finally
            {
                m_ActionLocker.Unlock();
            }
        }

        internal void RaiseAfterActionExecuted(ActionMetaItem action, IObject bo)
        {
            if (OnAfterActionExecuted != null) OnAfterActionExecuted(ParentLayout != null ? ParentLayout.ParentBasePanel : null, action, bo);
        }

        private static List<object> CreatePanelGridParameters(IBaseListPanel listPanel)
        {
            var item = listPanel.FocusedItem;
            var key = (item == null)
                             ? null
                             : item.Key;
            return new List<object> { key, item, listPanel };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GridPanelItem_DeleteValidation(object sender, ValidationEventArgs args)
        {
            // todo: show warning according to object type
            if (!String.IsNullOrEmpty(args.MessageId))
            {
                ErrorForm.ShowWarning(args.MessageId, "Can't delete object");
            }
            else
            {
                if (args.FieldName == "_on_delete")
                {
                    if (args.ShouldAsk)
                    {
                        if (ErrorForm.ShowConfirmationDialog(null, BvMessages.Get("msgConfirmReferenceDeleting", "You are going to delete the reference value which is used in the system already. Do you want to delete reference value?"), null) == DialogResult.Yes)
                        {
                            args.Continue = true;
                        }
                    }
                    else
                        ErrorForm.ShowWarning("msgCantDelete", "Can't delete object");
                }
                else
                    ErrorForm.ShowWarning("msgErrorDelete", "Error occurred while deleting object");
            }
        }

        /// <summary>
        /// Типовое действие "Закрыть форму"
        /// </summary>
        private void ActionCloseForm(DialogResult result = DialogResult.Cancel)
        {
            if (ParentLayout.ParentBasePanel != null)
            {
                BaseFormManager.Close(ParentLayout.ParentBasePanel as IApplicationForm, result);
            }
        }

        /// <summary>
        /// Типовое действие "Сохранить"
        /// </summary>
        /// <param name="action"> </param>
        /// <param name="saveOnly">true-выполняется действие Save, false-выполняется действие OK</param>
        private bool ActionSave(ActionMetaItem action, bool saveOnly)
        {
            if (!BusinessObject.HasChanges) return true;
            var form = FindForm();
            if (form != null)
                form.BringToFront();
            var canProceed = saveOnly ? WinUtils.ConfirmSave() : WinUtils.ConfirmOk();
            if (!canProceed) return false;
            using (new WaitDialog(WaitDialogType.FormSaving))
            {
                using (var manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    return action.RunAction(manager, BusinessObject).result;
                }

            }
        }

        /// <summary>
        /// Типовое действие "Отмена" (подтверждение отмены)
        /// </summary>
        /// <returns></returns>
        private bool ActionCanCancel()
        {
            return ConfirmCancel(BusinessObject, FindForm());
        }

        public static bool ConfirmCancel(IObject bo, Form owner)
        {
            return !((bo != null) && (bo.IsNew || bo.HasChanges) && !WinUtils.ConfirmCancel(owner));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual Control AddAction(ActionMetaItem action)
        {
            Control actionControl = null;

            if (!IsActionVisible(action)) return null;

            //если это кастомная кнопка, то определим для неё функцию
            if (action.ActionType.Equals(ActionTypes.Action) && (BasePanel != null))
            {
                var kvpFirst = BasePanel.GetFirstUIFunc(action.Name);
                var kvpSecond = BasePanel.GetSecondUIFunc(action.Name);
                if (kvpFirst.Key != null) action.AddFirstUIFunc(kvpFirst.Key, kvpFirst.Value);
                if (kvpSecond.Key != null) action.AddSecondUIFunc(kvpSecond.Key, kvpSecond.Value);
            }

            //если не надо группировать действие, то просто рендерим его как кнопку
            if (String.IsNullOrEmpty(action.Container) && action.ActionType != ActionTypes.Container)
            {
                #region Создание обычной кнопки

                //создаём кнопки для каждого действия

                var button = new SimpleButton
                                 {
                                     Name = String.Format("btn{0}", action.Name),
                                 };

                InitButton(button, action);
                button.Tag = action;
                button.Click += OnButtonClick;
                m_Buttons.Add(button); //TODO может быть добавить дополнительную очистку списка в Dispose

                actionControl = button;

                #endregion

                if (BusinessObject != null)
                {
                    SetButtonEnbabled(button, BusinessObject, action);
                    BusinessObject.PropertyChanged += (sender, args) => SetButtonEnbabled(button, BusinessObject, action);
                    BusinessObject.AfterPost += (sender, args) => SetButtonEnbabled(button, BusinessObject, action);
                }

                if (!Controls.Contains(actionControl))
                    Controls.Add(actionControl);
            }

            return actionControl;
        }

        protected void SetButtonEnbabled(Control button, IObject bo,  ActionMetaItem action)
        {
            button.Enabled = action.IsEnable(bo, bo.GetPermissions());
        }
        private void InitButton(SimpleButton btn, ActionMetaItem action)
        {
            btn.Text = GetMessage(action.CaptionId(BusinessObject, Permissions));
            btn.Image = ImagesStorage.Get(action.IconId(BusinessObject, Permissions));
            if (!string.IsNullOrEmpty(action.TooltipId(BusinessObject, Permissions)))
            {

                var superToolTip = new SuperToolTip();
                //TODO получать текст специальным методом
                superToolTip.Items.Add(GetMessage(action.TooltipId(BusinessObject, Permissions)));
                btn.SuperTip = superToolTip;
            }
            SetControlWidth(btn);

        }

        /// <summary>
        /// Добавляет произвольный контрол на панель
        /// </summary>
        /// <param name="ctl"> </param>
        /// <param name="position"> </param>
        public virtual Control AddCustomControl(Control ctl, int position = -1)
        {
            if (position >= 0 && position < m_Buttons.Count)
                m_Buttons.Insert(position, ctl);
            else
                m_Buttons.Add(ctl);
            if (!Controls.Contains(ctl))
                Controls.Add(ctl);
            return ctl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual bool IsActionVisible(ActionMetaItem action)
        {
            if (BaseGridPanel != null && action.PanelType == ActionsPanelType.Top)
            {
                switch (BaseGridPanel.SelectMode)
                {
                    case SelectMode.MultiSelect:
                        return action.ActionType == ActionTypes.SelectAll || action.ActionType == ActionTypes.Select;
                    case SelectMode.SimpleSelect:
                        return action.ActionType == ActionTypes.Select;
                    default:
                        if (action.ActionType == ActionTypes.SelectAll || action.ActionType == ActionTypes.Select)
                            return false;

                        return action.IsVisible(BusinessObject, Permissions);
                }

            }
            return action.IsVisible(BusinessObject, Permissions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        protected void SetControlWidth(Control control)
        {
            if (control is BaseButton)
            {
                var size = (control as BaseButton).CalcBestSize();
                size.Width += 8;
                if (size.Width < m_ButtonMinWidth)
                    size.Width = m_ButtonMinWidth;
                control.Size = size;
                return;
            }
            var g = control.CreateGraphics();
            var buttonWidth = (int)g.MeasureString(control.Text, control.Font).Width;
            //если у контрола есть картинка, то требуется учесть её ширину
            control.Size = new Size(buttonWidth >= m_ButtonMinWidth ? buttonWidth : m_ButtonMinWidth, m_ButtonHeight);
        }

        /// <summary>
        /// Типичная высота кнопки
        /// </summary>
        internal static int m_ButtonHeight = 23;

        /// <summary>
        /// Минимальная ширина кнопки
        /// </summary>
        internal static int m_ButtonMinWidth = 75;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnActionPanelResize(object sender, EventArgs e)
        {
            RecalculateActionsPositions();
        }

        [Browsable(false), Localizable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static BaseStringsStorage Messages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="resourceValue"></param>
        /// <returns></returns>
        public string GetMessage(string resourceKey, string resourceValue = null)
        {
            string s = BvMessages.Instance.GetString(resourceKey, resourceValue, null, GetViewNameForSplittedResources());
            if (BvMessages.Instance.IsValueExists)
                return s;
            if (Messages != null)
            {
                s = Messages.GetString(resourceKey, resourceValue, null, GetViewNameForSplittedResources());
                return s;
            }
            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected static DbManagerProxy CreateDbManagerProxy()
        {
            Dbg.Assert(DbManagerFactory.Factory != null, "DbManagerFactory.Factory not initialized");
            // ReSharper disable PossibleNullReferenceException
            return DbManagerFactory.Factory.Create(ModelUserContext.Instance);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="parameters"> </param>
        /// <returns></returns>
        public bool PerformAction(string actionName, List<object> parameters)
        {
            if (BusinessObject == null) return true;
            var acc = BusinessObject.GetAccessor() as IObjectMeta;
            if (acc == null)
                return false;
            var action = acc.Actions.Find(c => c.Name.Equals(actionName));
            if (action != null)
            {
                //sometimes this method is called inside other action execution code
                //when actions are locked already
                //In this case we unlock action and then lock it again
                var lockAfterRun = false;
                if (m_ActionLocker.Locked)
                {
                    lockAfterRun = true;
                    m_ActionLocker.Unlock();
                }
                RunAction(action, parameters);
                if (lockAfterRun)
                    m_ActionLocker.Lock();
                return true;
            }
            return false;
        }
        public void RefreshActionButtons(bool forceReadOnly = false)
        {
            foreach (Control ctl in Controls)
            {
                var action = ctl.Tag as ActionMetaItem;
                if (action != null)
                {
                    if (!IsStandardAction(action))
                        InitButton(ctl as SimpleButton, action);
                    SetButtonEnbabled(ctl, BusinessObject, action);
                }
                RecalculateActionsPositions();
                if (BaseGridPanel != null)
                {
                    BaseGridPanel.RefreshFocusedItem();
                }

            }
        }
        private bool IsStandardAction(ActionMetaItem action)
        {
            return action.ActionType == ActionTypes.Ok || action.ActionType == ActionTypes.Cancel ||
                   action.ActionType == ActionTypes.Save || action.ActionType == ActionTypes.Refresh ||
                   action.ActionType == ActionTypes.Close || action.ActionType == ActionTypes.Select ||
                   action.ActionType == ActionTypes.SelectAll;
        }
        public void SetButtonVisibility(string actionName, bool visible)
        {
            //этим методом следует пользоваться, только если никак иначе не реализовать функционал
            foreach (var button in m_Buttons)
            {
                var ami = button.Tag as ActionMetaItem;
                if ((ami != null) && ami.Name.Equals(actionName))
                {
                    button.Visible = visible;
                    break;
                }
            }
            //пересчет положения кнопок
            RecalculateActionsPositions();
        }

        #region ITranslationView
        public override void ApplyResources(string cultureCode)
        {
            ResourcesList.Clear();
            foreach (Control ctl in Controls)
            {
                var action = ctl.Tag as ActionMetaItem;
                if (action != null)
                {
                    string s = action.CaptionId(BusinessObject, Permissions);
                    var resValue = CommonResourcesCache.Get(CommonResource.BvMessages.ToString(), cultureCode, s, GetViewNameForSplittedResources());
                    var propName = TranslationToolHelper.GetPropertyName(ctl.Name, TranslationToolHelper.TextPropName);
                    if (resValue == null && Messages != null)
                    {
                        resValue = CommonResourcesCache.Get(Messages.GetType().Name, cultureCode, s, GetViewNameForSplittedResources());
                    }
                    if (resValue != null)
                    {
                        ResourcesList.Add(propName, resValue);
                        ctl.Text = resValue.Value.ToString();

                        if (!EditableControlsList.ContainsKey(propName))
                            EditableControlsList.Add(propName, ctl);
                    }
                }
                else if (BasePanel != null && !(ctl is TranslationButton))
                {
                    TranslationToolHelperWinClient.ApplyResources(ctl, BasePanel as ITranslationView, cultureCode);
                }

            }

        }
        public override bool SaveChanges(Dictionary<object, DesignElement> changes, string cultureCode)
        {
            bool res = TranslationToolHelperWinClient.SaveViewChanges(this, CommonResource.BvMessages.ToString(), changes, cultureCode, false);
            if (res && Messages != null)
                res = TranslationToolHelperWinClient.SaveViewChanges(this, Messages.GetType().Name, changes, cultureCode, false);
            //if (res && BasePanel!=null)
            //    res = TranslationToolHelperWinClient.SaveViewChanges(BasePanel as ITranslationView, BasePanel.GetType().Name, changes, cultureCode, false);

            return res;
        }
        public override string GetKeyForComponent(Component component, DesignElement designType)
        {
            var ctl = component as Control;
            if (ctl != null)
            {
                var action = ctl.Tag as ActionMetaItem;
                if (action != null)
                {
                    return action.CaptionId(BusinessObject, Permissions);
                }
            }
            return base.GetKeyForComponent(component, designType);
        }
        public override string GetResourceNameForComponent(Component component, DesignElement designType)
        {
            var key = GetKeyForComponent(component, designType);
            if (!string.IsNullOrEmpty(ResourceSplitter.Read(GetViewNameForSplittedResources(), key)))
                return string.Empty;
            BvMessages.Get(key);
            if (BvMessages.Instance.IsValueExists)
            {
                return "BvMessages";
            }
            if (Messages != null)
            {
                Messages.GetString(key);
                if (Messages.IsValueExists)
                    return Messages.GetType().Name;
            }
            var parentView = ParentLayout.ParentBasePanel as ITranslationView;
            if (parentView != null)
                return parentView.GetResourceNameForComponent(component, designType);
            return base.GetResourceNameForComponent(component, designType);
        }
        public override DesignElement GetDesignTypeForComponent(Component component)
        {
            return DesignElement.Caption;
        }

        public override string GetViewNameForSplittedResources()
        {
            return string.Format("{0}.{1}", ParentLayout.ParentBasePanel.GetType().Name, GetType().Name);
        }
        public override string GetViewNameForResourceUsage()
        {
            if (ParentLayout.ParentBasePanel.RootPanel == ParentLayout.ParentBasePanel)
                return GetType().Name;
            return string.Format("{0}.{1}", ParentLayout.ParentBasePanel.GetType().Name, GetType().Name);
        }

        #endregion
    }
}
