using System.Collections.Generic;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using EIDSS.FlexibleForms.Components;
using EIDSS.FlexibleForms.DataBase;
using System.Linq;

namespace EIDSS.FlexibleForms.Helpers
{
    /// <summary>
    /// ������ � ���������
    /// </summary>
    public static class StubHelper
    {
        /// <summary>
        /// �������� �������� �� �������� ��-�� ��������
        /// </summary>
        private const int DeltaStub = 50;

        /// <summary>
        /// ��������� �������� � ������ ��� ������������� (����������������� ��������)
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplateTarget">������, � ������� ����� �������� �������</param>
        /// <param name="listStubInfo">���� ��� AggregateCaseSection - Version</param>
        /// <returns></returns>
        public static int IncludeStub(this DbService mainDbService, long idfsFormTemplateTarget, List<StubInfo> listStubInfo)
        {
            //��������������� ������ ��� ����������� �������� ��������
            var stubColumns = new List<long>();
            if (listStubInfo.Count == 0) return 0;

            #region ��������� ������������������ ��������

            var versions = new List<long>();
            foreach (var version in listStubInfo)
            {
                if (!versions.Contains(version.IdfVersion)) versions.Add(version.IdfVersion);
            }

            //������ ������, � ������� StubInfo ����� ���� ���� ������� � ���� ������
            var lsi = listStubInfo[0];
            mainDbService.LoadPredefinedStub(lsi.MatrixType, versions, lsi.IdfsFormTemplate);

            foreach (var stubInfo in listStubInfo)
            {
                //��������� ������ �������� ������, ���� ���������� ���������� �������� � ���� �������
                //���� �� ������� ������������ ��������� ������, �� ����������� �� ��� ����������������� �������
                //������ ��� �� ��������� (�������� � RestoreSections), ������ ������ ������������ ��������� �������� �������
                //TODO ��������, ����� ������ ������, ���� �� ������� ��������� ������
                mainDbService.SetStubLoaded(stubInfo.IdfsFormTemplate, stubInfo.IdfsSectionTable);
                //����� �������� ����� ������������ �������� � �������� �������� �������
                if (!stubInfo.IdfsFormTemplate.Equals(idfsFormTemplateTarget))
                {
                    mainDbService.SetStubLoaded(idfsFormTemplateTarget, stubInfo.IdfsSectionTable);
                }
            }

            //����� ����������� ������������� �������� ���������� ��������. ���������, ��� ���������� ����� ���� �� ������ deltaStub.
            //������� �������� ����� ���� � ������� ��������� ����������, � ������� ����� �� ������� � ������ ����������� ����������,
            //����� ��� ���������� ���������� �� ����������� ������� ������ ��� �����
            //� ����������, ��������� �������������� � �������, ������� ������ �������������
            var intOrder = -DeltaStub;
            //���������� ��� ����������� ������� ��� ���������� ��������. ������������� ������� � ������ �����������.
            
            foreach (FlexibleFormsDS.PredefinedStubRow stubRow in mainDbService.MainDataSet.PredefinedStub.Rows)
            {
                //������ ��������. �� ������ ����������� �������������� � ����������� ����������, ������ ��� �� �������� �� ��������� � ���� �� ���� �����
                //TODO ������ ������, ���� ��� �� ���
                var parametersRow = mainDbService.GetParameterRow(stubRow.idfsParameter);
                //���� ����� �������� ��� ������������ � �������, �� �� ��������� ��� ��������
                //��� ������� ��������, ��������� PredefinedStub ��������������� ���������� ����������
                
                if (!stubColumns.Contains(parametersRow.idfsParameter)) stubColumns.Add(parametersRow.idfsParameter); 
                if (mainDbService.GetParameterTemplateRow(parametersRow.idfsParameter, idfsFormTemplateTarget) != null) continue;

                //������, ��� �������� ����� ���������� � ���������� ��������� ������
                parametersRow.idfsSection = stubRow.idfsSection;
                parametersRow.idfsFormTemplate = idfsFormTemplateTarget;
                parametersRow.intOrder = intOrder;
                //������ �� ������ ���������� ��������� ������ � ������� (���������� �� �����, ������ ��� �������� ����� ������� � ��������� ������)
                mainDbService.CreateParameterTemplateRow(parametersRow, idfsFormTemplateTarget);
                intOrder++;                
            }

            #endregion

            return stubColumns.Count;
        }

        /// <summary>
        /// ��������� �������� � ������ ��� ������������� (����������������� ��������)
        /// </summary>
        /// <param name="presetStubs"></param>
        /// <param name="mainDbService"></param>
        /// <param name="idfVersion"></param>
        /// <param name="idfsFormTemplate">������, � ������� ����� ���������� ������ ��� ���������� ��������, � � ������� ����� �������� �������</param>
        /// <param name="idfsMatrixType"></param>
        public static int IncludeStub(this IEnumerable<long> presetStubs, DbService mainDbService, long idfsFormTemplate, long idfVersion, long idfsMatrixType)
        {
            return mainDbService.IncludeStub(idfsFormTemplate, GetStubInfo(presetStubs, idfsFormTemplate, idfVersion, idfsMatrixType));
        }

        /// <summary>
        /// ���������� �������������� ������ �� AggregateCaseSection � StubInfo
        /// </summary>
        /// <param name="presetStubs"></param>
        /// <returns></returns>
        public static List<StubInfo> GetStubInfo(this IEnumerable<long> presetStubs)
        {
            return presetStubs.GetStubInfo(null, null, null);
        }

        /// <summary>
        /// ���������� �������������� ������ �� AggregateCaseSection � StubInfo
        /// </summary>
        /// <param name="presetStubs"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfVersion"></param>
        /// <param name="idfsMatrixType"></param>
        /// <returns></returns>
        public static List<StubInfo> GetStubInfo(this IEnumerable<long> presetStubs, long? idfsFormTemplate, long? idfVersion, long? idfsMatrixType)
        {
            var result = new List<StubInfo>();

            foreach (var stub in presetStubs)
            {
                var stubInfo = new StubInfo {IdfsSectionTable = stub};
                if (idfsFormTemplate.HasValue) stubInfo.IdfsFormTemplate = idfsFormTemplate.Value; //�� ����� �� ������� ����� � ���� �� ��������� �������
                if (idfVersion.HasValue) stubInfo.IdfVersion = idfVersion.Value;
                if (idfsMatrixType.HasValue) stubInfo.MatrixType = idfsMatrixType.Value;
                result.Add(stubInfo);
            }
            return result;
        }

        /// <summary>
        /// ������������� ��������
        /// </summary>
        /// <param name="listAggregateCaseSection"></param>
        /// <param name="sectionTableList"></param>
        /// <param name="stubLength"></param>
        public static void PostProcessingStub(this List<long> listAggregateCaseSection, Dictionary<long, SectionTable> sectionTableList, int stubLength)
        {
            var listStubInfo = listAggregateCaseSection.GetStubInfo(null, null, null);
            PostProcessingStub(listStubInfo, sectionTableList, stubLength);
        }

        /// <summary>
        /// ������������� ��������
        /// </summary>
        /// <param name="sectionTableList"></param>
        /// <param name="listStubInfo"></param>
        /// <param name="stubLength"></param>
        public static void PostProcessingStub(this List<StubInfo> listStubInfo, Dictionary<long, SectionTable> sectionTableList, int stubLength)
        {
            if (listStubInfo.Count == 0) return;
            foreach (var stubInfo in listStubInfo)
            {
                var rootSectionTable = sectionTableList[stubInfo.IdfsSectionTable];
                //��������� ����������� ���������� ����� �����
                rootSectionTable.GridViewMain.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
                rootSectionTable.GridViewMain.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
                //��������� (�������) �������� �������� �� ��������������. ��� ������ ����� �����.
                for (int i = 0; i < stubLength; i++)
                {
                    //������� �������� ������ �������������
                    foreach (GridColumn column in rootSectionTable.GridViewMain.Bands[i].Columns)
                    {
                        column.OptionsColumn.AllowEdit = false;
                        //��������� ������� ���������, ����� �� ���� ��������
                        if (i == stubLength - 1) column.Fixed = FixedStyle.Left;
                    }
                }
            }
        }

        /// <summary>
        /// �������� � ������� ���������������� ������ ������ ��������
        /// </summary>
        public static void IncludeStubDataInUserData(this DbServiceUserData mainDbService, long? templateID, long? observationID, long? versionMatrixStub, List<SummaryRow> summaryRowList, out List<FlexibleFormsDS.PredefinedStubRow> predefinedStubRows)
        {
            predefinedStubRows = null;
            if (!templateID.HasValue || !observationID.HasValue) return;

            //������� ������ ������� (�� ������ ����� ������ �������� � ����� ������ ������ ��� ������ observation
            mainDbService.DeleteStubData(templateID.Value, observationID.Value);
            predefinedStubRows =
                versionMatrixStub.HasValue
                    ? mainDbService.GetPredefinedStubRows(versionMatrixStub.Value).ToList()
                    : ((FlexibleFormsDS.PredefinedStubRow[])mainDbService.MainDataSet.PredefinedStub.Select()).ToList();

            //������ �� �������� ��� ������ ���� ���������, ��������� �� ��� �� ����������������
            //��������� ������� �������� �������� �����
            if (summaryRowList != null)
            {
                var idfsRows = new List<long>();
                foreach (var sr in summaryRowList)
                {
                    if (sr.idfRows.Count == 0) continue;
                    //��������� ������ ���� ������
                    var id = sr.idfRows[0];
                    //if (idfsRows.Contains(id)) continue;
                    idfsRows.Add(id);
                }
                for (var i = predefinedStubRows.Count - 1; i >= 0; i--)
                {
                    var index = idfsRows.IndexOf(predefinedStubRows[i].idfRow);
                    var psr = predefinedStubRows[i];
                    if (index >= 0)
                    {
                        psr.NumRow = summaryRowList[index].NumRow;
                    }
                    else
                    {
                        predefinedStubRows.Remove(psr);
                    }
                }
            }
            foreach (var predefinedStubRow in predefinedStubRows)
            {
                //�������� ������ �� ��������
                mainDbService.ChangeValue(observationID.Value, templateID.Value, predefinedStubRow.idfsParameter, predefinedStubRow.idfRow, predefinedStubRow.NumRow, predefinedStubRow.idfsParameterValue, predefinedStubRow.strNameValue);
            }
        }
    }


    /// <summary>
    /// ��������������� ��������� ��� ������������� � ������� ���������
    /// </summary>
    public class StubInfo
    {
        /// <summary>
        /// ������, � ������� ��������� ��������� ������ � ������� ���������
        /// </summary>
        public long IdfsFormTemplate { get; set; }
        /// <summary>
        /// ��������� ������, � ������� ��������� �������
        /// </summary>
        public long IdfsSectionTable { get; set; }
        /// <summary>
        /// ������ ��������
        /// </summary>
        public long IdfVersion { get; set; }
        /// <summary>
        /// Observation, � �������� ��������� �������
        /// </summary>
        public long IdfObservation { get; set; }

        public long MatrixType { get; set; }
    }

}
