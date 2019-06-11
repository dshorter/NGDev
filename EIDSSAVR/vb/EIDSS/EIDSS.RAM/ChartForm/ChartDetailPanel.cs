﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using bv.common.Configuration;
using bv.common.Core;
using bv.winclient.Core;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraPrinting;
using eidss.avr.BaseComponents;
using eidss.avr.BaseComponents.Views;
using eidss.model.Avr.Chart;
using eidss.model.AVR.DataBase;
using eidss.model.Resources;
using eidss.model.WindowsService.Serialization;

namespace eidss.avr.ChartForm
{
    public sealed partial class ChartDetailPanel : BaseAvrDetailPresenterPanel, IChartView
    {
        /// <summary>
        ///     В тихом режиме программа не спрашивает пользователя
        /// </summary>
        public bool SilenceMode { get; set; }

        private ChartPresenter m_ChartPresenter;

        public ChartDetailPanel()
        {
            try
            {
                InitializeComponent();
                if (IsDesignMode() || BaseSettings.ScanFormsMode)
                {
                    return;
                }
                m_ChartPresenter = (ChartPresenter) SharedPresenter[this];
                ChartControl.AppearanceName = "Nature Colors";
                ChartControl.PaletteName = "Nature Colors";
                ChartControl.PaletteBaseColorNumber = 0;
                ChartControl.SelectionMode = ElementSelectionMode.Single;
                ChartControl.CustomDrawSeriesPoint += ChartControl_CustomDrawSeriesPoint;
                ChartProperties = new AvrChartProperties();
                OnBeforePost += ChartDetailPanel_OnBeforePost;
                ChartTitle = XAxisTitle = String.Empty;
                SilenceMode = false;
                IsPosted = false;
                if (Localizer.IsRtl)
                {
                    SettingsNavBar.Dock = DockStyle.Right;
                    splitter.Dock = DockStyle.Right;
                }
            }
            catch (Exception ex)
            {
                if (BaseSettings.ThrowExceptionOnError)
                {
                    throw;
                }
                ErrorForm.ShowError(ex);
            }
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                eventManager.ClearAllReferences();
                if (m_ChartPresenter != null)
                {
                    if (m_ChartPresenter.SharedPresenter != null)
                    {
                        m_ChartPresenter.SharedPresenter.UnregisterView(this);
                    }
                    m_ChartPresenter.Dispose();
                    m_ChartPresenter = null;
                }

                if (disposing && (components != null))
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override bool HasChanges()
        {
            EndEdit();
            string currentXml = AvrChartPropertiesSerializer.Serialize(ChartProperties);
            return InitialXml != currentXml;
        }

        public bool IsPosted { get; private set; }

        private void ChartDetailPanel_OnBeforePost(object sender, EventArgs e)
        {
            DataTableCollection tables = baseDataSet.Tables;
            if (tables.Count > 0 && tables[0].Rows.Count > 0)
            {
                EndEdit();
                string xmlForPost = AvrChartPropertiesSerializer.Serialize(ChartProperties);
                byte[] zipForPost = BinaryCompressor.ZipString(xmlForPost);
                tables[0].Rows[0]["blbChartLocalSettings"] = zipForPost;
                IsPosted = true;
            }
        }

        private void EndEdit()
        {
            if (scrollableMain.Controls.Count > 0)
            {
                var settings = scrollableMain.Controls[0] as ISettingsPanel;
                if (settings != null)
                {
                    SaveDataFromSettingsPanel(settings);
                }
            }
        }

        public byte[] GetXml()
        {
            string xmlForPost = GetXmlString();
            byte[] zipForPost = BinaryCompressor.ZipString(xmlForPost);
            return zipForPost;
        }

        public string GetXmlString()
        {
            EndEdit();
            return AvrChartPropertiesSerializer.Serialize(ChartProperties);
        }

        public string ChartName
        {
            get { return IsDesignMode() ? String.Empty : ChartPlaceHolder.ChartName; }
            set
            {
                if (IsDesignMode())
                {
                    return;
                }
                ChartPlaceHolder.ChartName = value;
            }
        }

        private string ChartTitle { get; set; }
        private string XAxisTitle { get; set; }
        internal DataTable DataTableSource { get; set; }

        private const string SeriesName = "Series";
        private const string SeriesCaption = "Caption";
        private const string ArgumentName = "Arguments";
        private const string ValueName = "Values";
        private const string DataType = "DataType";


        private Dictionary<string, string> m_LabeltextPatterns = new Dictionary<string, string>();
        /// <summary>
        ///     Data source for chart control
        /// </summary>
        public object DataSource
        {
            get { return ChartControl.DataSource; }
            set
            {
                if (IsDesignMode())
                {
                    return;
                }
                var tbl = value as DataTable;
                if ((tbl != null) && (tbl.Columns.Count > 0))
                {
                    ChartTitle = tbl.TableName;
                    DataTableSource = new DataTable(ChartTitle);
                    DataTableSource.Columns.Add(SeriesName, typeof (string));
                    DataTableSource.Columns.Add(SeriesCaption, typeof (string));
                    DataTableSource.Columns.Add(ArgumentName, typeof (string));
                    DataTableSource.Columns.Add(ValueName, typeof (decimal));
                    DataTableSource.Columns.Add(DataType, typeof (Type));

                    XAxisTitle = tbl.Columns[0].Caption; //ColumnName
                    m_LabeltextPatterns.Clear();
                    for (int i = 1; i < tbl.Columns.Count; i++)
                    {
                        DataColumn column = tbl.Columns[i];
                        if (column.DataType.IsNumeric())
                        {
                            if (column.ExtendedProperties.ContainsKey("TextPattern") &&
                                column.ExtendedProperties["TextPattern"] != null)
                            {
                                m_LabeltextPatterns.Add(column.ColumnName, column.ExtendedProperties["TextPattern"].ToString());
                            }

                            foreach (DataRow row in tbl.Rows)
                            {
                                DataRow dtRow = DataTableSource.NewRow();
                                dtRow[SeriesName] = column.ColumnName; //ColumnName
                                dtRow[SeriesCaption] = column.Caption;
                                dtRow[ArgumentName] = row[0].ToString();
                                dtRow[DataType] = column.DataType;

                                object val = row[i];
                                bool badValue = Utils.IsEmpty(val) ||
                                                (val is double &&
                                                (double.IsInfinity((double)val) || 
                                                double.IsNaN((double)val) ||
                                                Math.Abs((double)val) > (double)Decimal.MaxValue||
                                                Math.Abs((double)val) < 1.0/(double)Decimal.MaxValue));

                                dtRow[ValueName] = badValue
                                    ? DBNull.Value
                                    : val;

                                DataTableSource.Rows.Add(dtRow);
                            }
                        }
                    }

                    ChartControl.DataSource = DataTableSource;
                    ChartControl.SeriesDataMember = SeriesName;
                    ChartControl.SeriesTemplate.ArgumentDataMember = ArgumentName;
                    ChartControl.SeriesTemplate.ValueDataMembers.AddRange(new[] {ValueName});

                   
                }
            }
        }

        

        public ChartControl ChartControl
        {
            get { return IsDesignMode() ? new ChartControl() : ChartPlaceHolder.ChartControl; }
        }

        public PrintingSystem PrintingSystem
        {
            get { return printingSystem; }
        }

        internal AvrChartProperties ChartProperties { get; set; }

        public void SetInitialXml(string initialXml)
        {
            InitialXml = initialXml;
            if (!String.IsNullOrEmpty(InitialXml))
            {
                ChartProperties = AvrChartPropertiesSerializer.Deserialize(InitialXml);
                foreach (SeriesProperties series in ChartProperties.Series)
                {
                    series.SetParentSeries();
                }
            }
            else
            {
                InitialXml = AvrChartPropertiesSerializer.Serialize(ChartProperties);
            }
            SetupChart();
        }

        private void ApplyToAllSeriesMethod(ViewType vt, object properties)
        {
            for (int index = 0; index < ChartControl.Series.Count; index++)
            {
                SeriesProperties props = ChartProperties.Series[index];
                Series series = ChartControl.Series[index];

                if (props.ViewType != (int)vt)
                {
                    series.ChangeView(vt);
                    props.ViewType = (int)vt;
                }


                var barProps = properties as IBarProperties;
                var lineProps = properties as ILineProperties;
                var pointProps = properties as PointProperties;
                var pieProps = properties as IPieProperties;

                switch (vt)
                {
                    case ViewType.Bar:
                        props.Bar.CopyFrom(barProps);
                        break;
                    case ViewType.StackedBar:
                        props.StackedBar.CopyFrom(barProps);
                        break;
                    case ViewType.FullStackedBar:
                        props.FullStackedBar.CopyFrom(barProps);
                        break;
                    case ViewType.Point:
                        props.Point.CopyFrom(pointProps);
                        break;
                    case ViewType.Line:
                        props.Line.CopyFrom(lineProps);
                        break;
                    case ViewType.Spline:
                        props.Spline.CopyFrom(lineProps);
                        break;
                    case ViewType.StepLine:
                        props.StepLine.CopyFrom(lineProps);
                        break;
                    case ViewType.Area:
                        props.Area.CopyFrom(barProps);
                        break;
                    case ViewType.Pie:
                        props.Pie.CopyFrom(pieProps);
                        break;
                }

                SetupSeries(series, props);
            }
        }

        public void ChangeChartTypeForAllSeries(DBChartType chartType)
        {
            //количество серий и количестве свойств серий должно совпадать
            if (ChartControl.Series.Count != ChartProperties.Series.Count)
            {
                return;
            }

            ViewType newViewType = ChartPresenter.GetChartType(chartType);
            for (int i = 0; i < ChartControl.Series.Count; i++)
            {
                Series ser = ChartControl.Series[i];
                ViewType serVt = SeriesViewFactory.GetViewType(ser.View);
                if (
                    (BaseSeriesSettings.IsPieType(newViewType) !=
                     BaseSeriesSettings.IsPieType(serVt))
                    ||
                    (BaseSeriesSettings.Is3DType(newViewType) !=
                     BaseSeriesSettings.Is3DType(serVt))
                    ||
                    (BaseSeriesSettings.IsFullStackedBarType(newViewType) !=
                     BaseSeriesSettings.IsFullStackedBarType(serVt)))
                {
                    //переделываем все серии на один тип
                    ChartControl.SeriesTemplate.ChangeView(newViewType);
                    SetupDiagram();
                    break;
                }
            }

            for (int index = 0; index < ChartControl.Series.Count; index++)
            {
                Series series = ChartControl.Series[index];
                series.ChangeView(newViewType);
                SeriesProperties seriesProperties = ChartProperties.Series[index];

                //меняем на заданный тип, только если это не массовый пересчёт
                //if (!needConvertAllSeries) seriesProperties.ViewType = (int)newViewType;

                seriesProperties.ViewType = (int) newViewType;
                SetupSeries(series, seriesProperties);
            }
        }

        protected override void DefineBinding()
        {
            /*
            var tables = baseDataSet.Tables;
            byte[] zip = null;
            if (tables.Count > 0 && tables[0].Rows.Count > 0)
            {
                zip = tables[0].Rows[0]["blbChartLocalSettings"] as byte[];
                if (zip != null)
                {
                   
                }
            }

            SetInitialXml(zip != null ? BinaryCompressor.UnzipString(zip) : AvrChartPropertiesSerializer.Serialize(ChartProperties));
            */
            ChartControl.ObjectSelected += ChartControl_ObjectSelected;
        }

        public byte[] ExportToJpgBytes()
        {
            using (var stream = new MemoryStream())
            {
                ChartControl.ExportToImage(stream, ImageFormat.Jpeg);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                var buffer = new byte[stream.Length];
                int readed = stream.Read(buffer, 0, (int) stream.Length);
                Debug.Assert(stream.Length == readed, "not all bytes readed");

                return buffer;
            }
        }

        private string InitialXml { get; set; }

        private void ChartControl_ObjectSelected(object sender, HotTrackEventArgs e)
        {
            ShowSettingsPanel(e.Object);
        }

        internal void ShowSettingsPanel(object o)
        {
            EndEdit();
            int index = -1;
            if (o is Series)
            {
                var series = (Series) o;
                o = series.View;
                index = ChartControl.Series.IndexOf(series);
            }
            else if (o is AxisY)
            {
                //TODO проверить для не-XY диаграммы
                index = 0; //основная ось Y (может не быть на XY диаграмме)
            }
            else
            {
                var axisYBase = o as AxisYBase;
                if (axisYBase != null)
                {
                    index = ((XYDiagram) ChartControl.Diagram).SecondaryAxesY.IndexOf(axisYBase) + 1; //потому что 0 индекс у Primary Axis Y
                }
            }
            ShowSettingsPanel(o, index);
        }

        /// <summary>
        /// </summary>
        /// <param name="o">Объект, на который пришёлся клик</param>
        /// <param name="index">Если кликнули на серию, то индекс этой серии</param>
        private void ShowSettingsPanel(object o, int index)
        {
            if (o is ChartTitle)
            {
                ShowSettings(new TitleSettings(this)
                {
                    PropertiesType = typeof (TitleProperties),
                    CurrentTitle = ChartPlaceHolder.ChartControl.Titles[0]
                }, ChartProperties.Title);
            }
            else if (o is AxisX)
            {
                ShowSettings(new XAxisSettings(this) {PropertiesType = typeof (XAxisProperties)}, ChartProperties.XAxis);
            }
            else if ((o is AxisYBase) && (index >= 0))
            {
                ShowSettings(new YAxisSettings(this) {PropertiesType = typeof (YAxisProperties), Index = index},
                    ChartProperties.YAxes[index]);
            }
            else if (o is Legend)
            {
                ShowSettings(new LegendSettings(this) {PropertiesType = typeof (LegendSettings)},
                    ChartProperties.LegendVisibility);
                //отступление от общего принципа
            }
                /*
        else if (o is SeriesLabelBase)
        {
            ShowSettings(new SeriesValueLabelsSettings(this) {PropertiesType = typeof (SeriesValueLabelsSettings)},
                ChartProperties.SeriesLabelsVisibility);
        }
        */
            else if ((o is AreaSeriesView) && (index >= 0))
            {
                var f = new AreaChartSettings(this)
                {
                    PropertiesType = typeof (AreaProperties),
                    AreaSeries = (AreaSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                };
                ShowSettings(f, ChartProperties.Series[index].Area);
            }
            else if ((o is FullStackedBarSeriesView) && (index >= 0))
            {
                var bar = new BarChartSettings(this)
                {
                    Title =
                        EidssFields.Get("FullStackedBarChartsSettingsCaption", "Full Stacked Bar Charts Settings"),
                    PropertiesType = typeof (FullStackedBarProperties),
                    BarSeries = (FullStackedBarSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                };
                ShowSettings(bar, ChartProperties.Series[index].FullStackedBar);
            }
            else if ((o is StackedBarSeriesView) && (index >= 0))
            {
                var bar = new BarChartSettings(this)
                {
                    Title =
                        EidssFields.Get("StackedBarChartsSettingsCaption", "Stacked Bar Charts Settings"),
                    PropertiesType = typeof (StackedBarProperties),
                    BarSeries = (StackedBarSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                };
                ShowSettings(bar, ChartProperties.Series[index].StackedBar);
            }
            else if ((o is BarSeriesView) && (index >= 0))
            {
                ShowSettings(new BarChartSettings(this)
                {
                    PropertiesType = typeof (BarProperties)
                    ,
                    BarSeries = (BarSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                },
                    ChartProperties.Series[index].Bar);
            }
            else if ((o is StepLineSeriesView) && (index >= 0))
            {
                var line = new LineChartSettings(this)
                {
                    Title = EidssFields.Get("StepLineChartsSettingsCaption", "StepLine Charts Settings"),
                    PropertiesType = typeof (StepLineProperties),
                    LineSeries = (StepLineSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                };
                ShowSettings(line, ChartProperties.Series[index].StepLine);
            }
            else if ((o is SplineSeriesView) && (index >= 0))
            {
                var line = new LineChartSettings(this)
                {
                    Title = EidssFields.Get("SplineChartsSettingsCaption", "Spline Charts Settings"),
                    PropertiesType = typeof (SplineProperties),
                    LineSeries = (SplineSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                };
                ShowSettings(line, ChartProperties.Series[index].Spline);
            }
            else if ((o is LineSeriesView) && (index >= 0))
            {
                ShowSettings(new LineChartSettings(this)
                {
                    PropertiesType = typeof (LineProperties)
                    ,
                    LineSeries = (LineSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                }, ChartProperties.Series[index].Line);
            }
            else if ((o is PointSeriesView) && (index >= 0))
            {
                ShowSettings(new PointChartSettings(this)
                {
                    PropertiesType = typeof (PointProperties)
                    ,
                    PointSeries = (PointSeriesView) o,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                }, ChartProperties.Series[index].Point);
            }
            else if ((o is PieSeriesView) && (index >= 0))
            {
                ShowSettings(new PieChartSettings(this)
                {
                    PropertiesType = typeof (PieProperties)
                    ,
                    PieSeries = ChartControl.Series[index]
                    ,
                    Index = index,
                    ApplyToAllSeriesRoutines = ApplyToAllSeriesMethod
                }, ChartProperties.Series[index].Pie);
            }
        }

        private void SaveDataFromSettingsPanel(ISettingsPanel panel)
        {
            /*
            if (panel.PropertiesType == typeof (LegendSettings))
            {
                ChartProperties.LegendVisibility = (bool) panel.ToProperties();
            }
            else if (panel.PropertiesType == typeof (SeriesValueLabelsSettings))
            {
                ChartProperties.SeriesLabelsVisibility = (bool) panel.ToProperties();
            }
            else 
            */
            if (panel.PropertiesType == typeof (TitleProperties))
            {
                ChartProperties.Title = (TitleProperties) panel.ToProperties();
            }
            else if (panel.PropertiesType == typeof (XAxisProperties))
            {
                ChartProperties.XAxis = (XAxisProperties) panel.ToProperties();
            }
            else if (panel.PropertiesType == typeof (YAxisProperties))
            {
                ChartProperties.YAxes[panel.Index] = (YAxisProperties) panel.ToProperties();
            }
            else if (panel.PropertiesType == typeof (AreaProperties))
            {
                ChartProperties.Series[panel.Index].Area = (AreaProperties) panel.ToProperties();
            }
            else if (panel.PropertiesType == typeof (FullStackedBarProperties))
            {
                var obj = (IBarProperties) panel.ToProperties();
                ChartProperties.Series[panel.Index].FullStackedBar = (FullStackedBarProperties) obj;
            }
            else if (panel.PropertiesType == typeof (StackedBarProperties))
            {
                var obj = (IBarProperties) panel.ToProperties();
                ChartProperties.Series[panel.Index].StackedBar = (StackedBarProperties) obj;
            }
            else if (panel.PropertiesType == typeof (BarProperties))
            {
                var obj = (IBarProperties) panel.ToProperties();
                ChartProperties.Series[panel.Index].Bar = (BarProperties) obj;
            }
            else if (panel.PropertiesType == typeof (StepLineProperties))
            {
                var obj = (ILineProperties) panel.ToProperties();
                ChartProperties.Series[panel.Index].StepLine = (StepLineProperties) obj;
            }
            else if (panel.PropertiesType == typeof (SplineProperties))
            {
                var obj = (ILineProperties) panel.ToProperties();
                ChartProperties.Series[panel.Index].Spline = (SplineProperties) obj;
            }
            else if (panel.PropertiesType == typeof (LineProperties))
            {
                var obj = (ILineProperties) panel.ToProperties();
                ChartProperties.Series[panel.Index].Line = (LineProperties) obj;
            }
            else if (panel.PropertiesType == typeof (PointProperties))
            {
                ChartProperties.Series[panel.Index].Point = (PointProperties) panel.ToProperties();
            }
        }

        private void ShowSettings(ISettingsPanel panel, object properties)
        {
            //сохраняем настройки в объект
            if (scrollableMain.Controls.Count == 1)
            {
                EndEdit();
            }
            scrollableMain.SuspendLayout();
            scrollableMain.Controls.Clear();
            panel.Init(properties);
            var ctr = (Control) panel;
            scrollableMain.Controls.Add(ctr);
            ctr.Dock = DockStyle.Fill;
            scrollableMain.ResumeLayout();
        }

        /// <summary>
        /// </summary>
        public void SetupChart()
        {
            #region Актуализация серий

            //удаляем настройки серий, которых больше нет
            for (int index = ChartProperties.Series.Count; index > ChartControl.Series.Count; index--)
            {
                ChartProperties.Series.RemoveAt(index - 1);
            }

            //добавляем хранилища настроек для каждой серии
            var colorList = new[]
            {
                Color.FromArgb(235, 210, 155)
                , Color.FromArgb(190, 220, 145)
                , Color.FromArgb(230, 175, 150)
                , Color.FromArgb(195, 235, 170)
                , Color.FromArgb(210, 165, 150)
                , Color.FromArgb(140, 205, 140)
                , Color.FromArgb(215, 215, 135)
                , Color.FromArgb(235, 220, 165)
                , Color.FromArgb(165, 215, 190)
                , Color.FromArgb(235, 220, 190)
                , Color.FromArgb(190, 230, 225)
                , Color.FromArgb(250, 210, 195)
            };

            int colorIndex = 0;
            for (int index = ChartProperties.Series.Count; index < ChartControl.Series.Count; index++)
            {
                var series = new SeriesProperties();
                series.SetColors(colorList[colorIndex].ToArgb());
                ChartProperties.Series.Add(series);
                series.Index = ChartProperties.Series.Count - 1;
                colorIndex++;
                if (colorIndex > colorList.Length - 1)
                {
                    colorIndex = 0;
                }
            }

            #endregion

            if (!ChartProperties.Title.TextWasChanged)
            {
                ChartProperties.Title.Text = ChartTitle; //LayoutTitle.DataTitle;
            }

            if (!ChartProperties.XAxis.AxisProperties.Title.TextWasChanged)
            {
                ChartProperties.XAxis.AxisProperties.Title.Text = XAxisTitle; //LayoutTitle.RowTitle;
            }

            foreach (YAxisProperties axis in ChartProperties.YAxes)
            {
                if (!axis.AxisProperties.Title.TextWasChanged)
                {
                    axis.AxisProperties.Title.Text = String.Empty; //LayoutTitle.ColumnTitle;
                }
            }

            TitleSettings.SetupChart(ChartPlaceHolder.ChartControl.Titles[0], ChartProperties.Title);

            AxisX axisX = ChartPlaceHolder.AxisX;
            if (axisX != null)
            {
                axisX.VisualRange.Auto = true;
                axisX.VisualRange.AutoSideMargins = true;
                XAxisSettings.SetupChart(axisX, ChartProperties.XAxis);
            }

            AxisY axisY = ChartPlaceHolder.AxisY;
            var diagram = ChartPlaceHolder.ChartControl.Diagram as XYDiagram;
            if (axisY != null)
            {
                axisY.VisualRange.AutoSideMargins = true;

                if (ChartProperties.YAxes.Count == 0)
                {
                    //добавляем primary axis y, которая есть всегда
                    ChartProperties.YAxes.Add(new YAxisProperties());
                }

                if ((ChartProperties.YAxes.Count == 1) && (diagram != null))
                {
                    //также добавляем сразу одну вторичную ось, которая согласно задаче 9686 тоже есть всегда
                    ChartProperties.YAxes.Add(new YAxisProperties {Position = 1});
                    SecondaryAxisYCollection secondaryAxes = diagram.SecondaryAxesY;
                    secondaryAxes.Add(new SecondaryAxisY());
                }

                YAxisSettings.SetupChart(this, axisY, ChartProperties.YAxes[0]);
            }
            if (diagram != null)
            {
                SecondaryAxisYCollection secondaryAxes = ((XYDiagram) ChartPlaceHolder.ChartControl.Diagram).SecondaryAxesY;
                //если вторичных осей меньше, чем ранее сохранённых, то воссоздаём недостающие
                int cntAxis = secondaryAxes.Count;
                for (int index = cntAxis; index < ChartProperties.YAxes.Count - 1; index++)
                {
                    var a = new SecondaryAxisY();
                    a.VisualRange.AutoSideMargins = false;
                    //a.Range.SideMarginsEnabled = false;
                    secondaryAxes.Add(a);
                }
                //настраиваем каждую вторичную ось
                for (int index = 0; index < secondaryAxes.Count; index++)
                {
                    if (index + 1 > ChartProperties.YAxes.Count)
                    {
                        break;
                    }
                    YAxisSettings.SetupChart(this, secondaryAxes[index], ChartProperties.YAxes[index + 1]);
                }
            }

            ceLegendVisibility.CheckedChanged -= ceLegendVisibility_CheckedChanged;
            ceLegendVisibility.Checked = ChartProperties.LegendVisibility;
            ChartPlaceHolder.ChartControl.Legend.Visibility =
                ChartProperties.LegendVisibility ? DefaultBoolean.True : DefaultBoolean.False;
            ceLegendVisibility.CheckedChanged += ceLegendVisibility_CheckedChanged;

            ce3D.CheckedChanged -= ce3D_CheckedChanged;
            ce3D.Checked = false;
            if (ChartProperties.Series.Count > 0)
            {
                SeriesProperties serp = ChartProperties.Series[0];

                //либо все серии 3D, либо ни одна. Иначе никак.
                var vt = (ViewType) Enum.Parse(typeof (ViewType), serp.ViewType.ToString(CultureInfo.InvariantCulture));
                ce3D.Checked = vt.ToString().Contains("3D");
                if (ce3D.Checked)
                {
                    ChartControl.SeriesTemplate.ChangeView(vt);
                    SetupDiagram();
                }
                else if (BaseSeriesSettings.IsPieType(vt) || BaseSeriesSettings.IsFullStackedBarType(vt))
                {
                    //определяем, какие серии, нужно ли делать кольцевую диаграмму
                    ChartControl.SeriesTemplate.ChangeView(vt);
                    SetupDiagram();
                }
            }
            ce3D.CheckedChanged += ce3D_CheckedChanged;

            //донастройка серий
            for (int index = 0; index < ChartControl.Series.Count; index++)
            {
                Series series = ChartControl.Series[index];
                SeriesProperties props = ChartProperties.Series[index];
                series.LabelsVisibility = props.SeriesLabelsVisibility ? DefaultBoolean.True : DefaultBoolean.False;
                series.ChangeView((ViewType) Enum.Parse(typeof (ViewType), props.ViewType.ToString(CultureInfo.InvariantCulture)));
                //
                SetupSeries(series, props);
            }

            SetupDiagram();
            SetSecondaryAxisVisibilityState();

            //встаём либо на первую серию, либо за заголовок диаграммы
            if (ChartControl.Series.Count > 0)
            {
                ShowSettingsPanel(ChartControl.Series[0].View, 0);
            }
            else if (ChartPlaceHolder.ChartControl.Titles.Count > 0)
            {
                ShowSettingsPanel(ChartPlaceHolder.ChartControl.Titles[0], 0);
            }

            //обновляем перечень серий в комбобоксе
            FillSeriesList();
        }

        internal void SetSecondaryAxisVisibilityState()
        {
            //чтобы вторичная ось была видимой, должна быть хотя бы одна серия, которая к ней привязана
            var diagram = ChartPlaceHolder.ChartControl.Diagram as XYDiagram;
            if (diagram == null)
            {
                return;
            }
            SecondaryAxisYCollection secondaryAxes = diagram.SecondaryAxesY;
            if (secondaryAxes.Count < 1)
            {
                return;
            }
            secondaryAxes[0].Visibility =
                ChartProperties.Series.Any(s => s.AxisYIndex == 1) ? DefaultBoolean.True : DefaultBoolean.False;
        }

        private void FillSeriesList()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Index", typeof (int)));
            dt.Columns.Add(new DataColumn("Text", typeof (string)));
            for (int index = 0; index < ChartControl.Series.Count; index++)
            {
                Series ser = ChartControl.Series[index];
                DataRow row = dt.NewRow();
                row["Index"] = index;
                row["Text"] = ser.LegendText;
                dt.Rows.Add(row);
            }
            leSeries.Properties.DataSource = dt;
            if (dt.Rows.Count > 0)
            {
                leSeries.EditValue = 0;
            }
        }

        internal void SetupSeries(Series series, SeriesProperties props)
        {
            SeriesViewBase view = series.View;
            var seriesView = view as AreaSeriesView;
            if (seriesView != null)
            {
                AreaChartSettings.SetupChart(seriesView, props.Area);
            }
            else
            {
                var barSeries = view as FullStackedBarSeriesView;
                if (barSeries != null)
                {
                    BarChartSettings.SetupChart(barSeries, props.FullStackedBar);
                }
                else
                {
                    var barSeriesView = view as StackedBarSeriesView;
                    if (barSeriesView != null)
                    {
                        BarChartSettings.SetupChart(barSeriesView, props.StackedBar);
                    }
                    else
                    {
                        var view1 = view as BarSeriesView;
                        if (view1 != null)
                        {
                            BarChartSettings.SetupChart(view1, props.Bar);
                        }
                        else
                        {
                            var lineSeries = view as StepLineSeriesView;
                            if (lineSeries != null)
                            {
                                LineChartSettings.SetupChart(lineSeries, props.StepLine);
                            }
                            else
                            {
                                var splineSeriesView = view as SplineSeriesView;
                                if (splineSeriesView != null)
                                {
                                    LineChartSettings.SetupChart(splineSeriesView, props.Spline);
                                }
                                else
                                {
                                    var lineSeriesView = view as LineSeriesView;
                                    if (lineSeriesView != null)
                                    {
                                        LineChartSettings.SetupChart(lineSeriesView, props.Line);
                                    }
                                    else
                                    {
                                        var pointSeries = view as PointSeriesView;
                                        if (pointSeries != null)
                                        {
                                            PointChartSettings.SetupChart(pointSeries, props.Point);
                                        }
                                        else
                                        {
                                            var pieSeriesView = view as PieSeriesView;
                                            if (pieSeriesView != null)
                                            {
                                                PieChartSettings.SetupChart(ChartControl, series
                                                    /*(SimplePointOptions)series.LegendPointOptions*/, props.Pie);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //всем сериям проставляем правильные заголовки
            series.CrosshairEnabled = DefaultBoolean.True;
            string caption = GetSeriesCaption(series.Name);
            series.LegendText = caption;
            //восстанавливаем настройки показа лейблов
            series.LabelsVisibility = props.SeriesLabelsVisibility ? DefaultBoolean.True : DefaultBoolean.False;
            //выставляем нужную ось
            var diagram = ChartPlaceHolder.ChartControl.Diagram as XYDiagram;
            if (diagram == null)
            {
                return;
            }
            SecondaryAxisYCollection secondaryAxes = diagram.SecondaryAxesY;

            AxisYBase axis = props.AxisYIndex == 0 ? diagram.AxisY : (AxisYBase) secondaryAxes[props.AxisYIndex - 1];
            var xyview = series.View as XYDiagramSeriesViewBase;
            if (xyview != null)
            {
                xyview.AxisY = axis;
            }
            SetupTextPatterns(series, caption, axis);
        }

        internal void SetupDiagram()
        {
            var diagram = ChartControl.Diagram as Diagram3D;
            if (diagram != null)
            {
                diagram.RuntimeRotation = true;
                diagram.RuntimeZooming = true;
                diagram.RuntimeScrolling = true;
            }
            else
            {
                var xyDiagram = ChartControl.Diagram as XYDiagram;
                if (xyDiagram != null)
                {
                    xyDiagram.AxisX.Title.Visibility = DefaultBoolean.True;
                    xyDiagram.AxisY.Title.Visibility = DefaultBoolean.True;
                    xyDiagram.EnableAxisXScrolling = true;
                    xyDiagram.EnableAxisXZooming = true;
                    xyDiagram.EnableAxisYScrolling = true;
                    xyDiagram.EnableAxisYZooming = true;
                }
            }
            /*
            else if (ChartControl.Diagram is SimpleDiagram)
                {
                var diagram = (SimpleDiagram)ChartControl.Diagram;
                }
            */
        }

        /// <summary>
        /// </summary>
        /// <param name="seriesName"></param>
        /// <returns></returns>
        internal string GetSeriesCaption(string seriesName)
        {
            string result = String.Empty;
            DataRow[] rows = DataTableSource.Select(String.Format("[{0}]='{1}'", SeriesName, seriesName));
            if (rows.Length > 0)
            {
                result = rows[0][SeriesCaption].ToString();
            }
            return result;
        }

        /// <summary>
        ///     Определяет индекс указанной оси в общем списке
        /// </summary>
        /// <returns></returns>
        public int GetAxisYIndex(AxisBase axisYBase)
        {
            int result;
            if (ChartPlaceHolder.AxisY == axisYBase)
            {
                result = 0;
            }
            else
            {
                SecondaryAxisYCollection secondaryAxes = ((XYDiagram) ChartPlaceHolder.ChartControl.Diagram).SecondaryAxesY;
                result = secondaryAxes.IndexOf(axisYBase) + 1;
            }
            return result;
        }

        public void ExportChart()
        {
            m_ChartPresenter.ExportChart();
        }

        public void PrintChart()
        {
            m_ChartPresenter.PrintChart(printingSystem);
        }

        private void ceLegendVisibility_CheckedChanged(object sender, EventArgs e)
        {
            ChartPlaceHolder.ChartControl.Legend.Visibility = ceLegendVisibility.Checked
                ? DefaultBoolean.True
                : DefaultBoolean.False;
            ChartProperties.LegendVisibility = ceLegendVisibility.Checked;
        }

        private void ce3D_CheckedChanged(object sender, EventArgs e)
        {
            EndEdit();
            if (ChartProperties.Series.Count == 0)
            {
                return;
            }
            //2D/3D ИД отличаются на 1
            //обходим все серии и переключаем их в другой режим
            int i = ce3D.Checked ? 1 : -1;
            SeriesProperties series0 = ChartProperties.Series[0];
            var vt = (ViewType) Enum.Parse(typeof (ViewType), series0.ViewType.ToString(CultureInfo.InvariantCulture));
            ViewType vt2;
            switch (vt)
            {
                case ViewType.Doughnut:
                case ViewType.Pie:
                    vt2 = ViewType.Pie3D;
                    break;
                case ViewType.Doughnut3D:
                case ViewType.Pie3D:
                    vt2 = ViewType.Pie;
                    break;
                case ViewType.Point:
                    vt2 = ViewType.Line3D;
                    break;
                default:
                    vt2 = ce3D.Checked ? ViewType.Line3D : ViewType.Line;
                    break;
            }

            ChartControl.SeriesTemplate.ChangeView(vt2);
            SetupDiagram();
            var diagram = ChartPlaceHolder.ChartControl.Diagram as XYDiagram;
            for (int index = 0; index < ChartProperties.Series.Count; index++)
            {
                SeriesProperties seriesProps = ChartProperties.Series[index];
                Series series = ChartControl.Series[index];
                vt = (ViewType) Enum.Parse(typeof (ViewType), seriesProps.ViewType.ToString(CultureInfo.InvariantCulture));
                if (vt == ViewType.Point)
                {
                    vt = ViewType.Line; //Point не поддерживает перевод в 3D и обратно, потому меняем его на Line
                }
                object dt = Enum.Parse(typeof (DBChartType), "chr" + vt);
                object dtNew = Enum.Parse(typeof (DBChartType), ((long) dt + i).ToString(CultureInfo.InvariantCulture));
                ViewType viewType = ChartPresenter.GetChartType((long) dtNew);

                series.ChangeView(viewType);
                seriesProps.ViewType = (int) viewType;
                SetupSeries(series, seriesProps);
                //возвращает серии правильную вторичную ось
                if (diagram != null)
                {
                    var view = series.View as XYDiagramSeriesViewBase;
                    if (view != null && seriesProps.AxisYIndex > 0)
                    {
                        view.AxisY = diagram.SecondaryAxesY[0];
                    }
                }
            }
            //показываем адекватный перечень доступных типов графика
            if (scrollableMain.Controls.Count > 0)
            {
                var settings = scrollableMain.Controls[0] as BaseSeriesSettings;
                if (settings != null)
                {
                    settings.FillChartTypes();
                    settings.SetParentSeriesViewType();
                    settings.SetChangeAxisAbility(!ce3D.Checked);
                }
            }
            //возвращает диаграмму в "нулевой зум"
            var diag = ChartControl.Diagram as XYDiagram;
            if (diag != null)
            {
                diag.AxisX.VisualRange.Auto = false;
                diag.AxisX.VisualRange.Auto = true;
                diag.AxisY.VisualRange.Auto = false;
                diag.AxisY.VisualRange.Auto = true;
            }
            //для 3D отсутствует панель настройки компонента
            scrollableMain.SuspendLayout();
            for (int j = scrollableMain.Controls.Count - 1; j >= 0; j--)
            {
                if (scrollableMain.Controls[j] is BaseSeriesSettings)
                {
                    ((BaseSeriesSettings) scrollableMain.Controls[j]).SwitchTo3D(ce3D.Checked);
                }
            }
            scrollableMain.ResumeLayout();
        }

        private void leSeries_EditValueChanged(object sender, EventArgs e)
        {
            if (leSeries.EditValue == null)
            {
                return;
            }
            int index = Convert.ToInt32(leSeries.EditValue);
            Series ser = ChartControl.Series[index];
            ShowSettingsPanel(ser);
        }

        #region Show Percents (or other text patterns)
        private void ChartControl_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            if (m_LabeltextPatterns.ContainsKey(e.Series.Name))// && m_LabeltextPatterns[e.Series.Name] == "p")
            {
                e.LabelText = String.Format("{0:" + m_LabeltextPatterns[e.Series.Name] + "}", e.SeriesPoint.Values[0]);
            }
        }

        private void SetupTextPatterns(Series series, string caption, AxisYBase axis)
        {
            if (m_LabeltextPatterns.ContainsKey(series.Name))// && m_LabeltextPatterns[series.Name] == "p")
            {
                string pattern = m_LabeltextPatterns[series.Name];
                series.Label.TextPattern = "{VP:" + pattern + "}";
                series.CrosshairLabelPattern = caption + ": {V:" + pattern + "}";
                axis.Label.TextPattern = "{V:" + pattern + "}";
            }
            else
            {
                series.CrosshairLabelPattern = caption + ": {V}";
            }
        }
        #endregion

        private void SettingsContainer_SizeChanged(object sender, EventArgs e)
        {
            if (Localizer.IsRtl && SettingsContainer.Parent is NavBarGroupControlContainerWrapper)
            {
                SettingsContainer.Parent.Left = 0;
                SettingsContainer.Parent.Width = SettingsContainer.Width;
                SettingsContainer.Left = 0;
            }

        }

        private void SettingsNavBar_NavPaneMinimizedGroupFormShowing(object sender, NavPaneMinimizedGroupFormShowingEventArgs e)
        {
            if (Localizer.IsRtl)
                e.NavPaneForm.Left = e.NavPaneForm.Left - e.NavBar.Width - e.NavPaneForm.Width;
        }

        private void SettingsNavBar_NavPaneStateChanged(object sender, EventArgs e)
        {
            splitter.Visible = SettingsNavBar.OptionsNavPane.NavPaneState == NavPaneState.Expanded;

        }
    }
}