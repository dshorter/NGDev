using System;
using System.Collections.Generic;
using System.Data;
using EIDSS.Enums;
using eidss.avr;
using eidss.avr.PivotComponents;
using eidss.avr.db.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bv.common.win;
using bv.tests.common;
using StructureMap;

namespace bv.tests.AVR.UnitTests
{
    [TestClass]
    public class FieldIconsReportTests :BaseReportTests
    {
        private static long m_Count;

        private IDisposable m_PresenterTransaction;

       
        [TestInitialize]
        public override void MyTestInitialize()
        {
            base.MyTestInitialize();
            m_PresenterTransaction = PresenterFactory.BeginSharedPresenterTransaction(m_Container, new BaseForm());
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            m_PresenterTransaction.Dispose();
            base.TestCleanup();
        }

       
        [TestMethod]
        public void QueryFieldTypeTest()
        {
            DataTable table = CreateLookupWithFieldType();
            Dictionary<string, SearchFieldType> types = QueryLookupHelper.GetFieldTypes(table);
            Assert.AreEqual(table.Rows.Count, types.Count);
            Assert.AreEqual(SearchFieldType.Bit, types["fBit"]);
            Assert.AreEqual(SearchFieldType.Date, types["fDate"]);
            Assert.AreEqual(SearchFieldType.Float, types["fFloat"]);
            Assert.AreEqual(SearchFieldType.ID, types["fID"]);
            Assert.AreEqual(SearchFieldType.Integer, types["fInteger"]);
            Assert.AreEqual(SearchFieldType.String, types["fString"]);
        }

        [TestMethod]
        public void QueryParamTypeTest()
        {
            DataTable table = CreateLookupWithFF();

            Dictionary<string, SearchFieldType> types = QueryLookupHelper.GetFieldTypes(table);

            Assert.AreEqual(SearchFieldType.Integer, types["fNumeric"]);
            Assert.AreEqual(SearchFieldType.Bit, types["fBoolean"]);
            Assert.AreEqual(SearchFieldType.String, types["fCaption"]);
            Assert.AreEqual(SearchFieldType.Date, types["fDate"]);
            Assert.AreEqual(SearchFieldType.Date, types["fDateTime"]);
            Assert.AreEqual(SearchFieldType.Integer, types["fMonths"]);
            Assert.AreEqual(SearchFieldType.String, types["fString"]);
            Assert.AreEqual(SearchFieldType.Integer, types["fNumericNatural"]);
            Assert.AreEqual(SearchFieldType.Integer, types["fNumericPositive"]);
            Assert.AreEqual(SearchFieldType.Integer, types["fNumericInteger"]);
        }

        [TestMethod]
        public void QueryFieldImageTest()
        {
            DataTable table = CreateLookupWithFieldType();
            Dictionary<string, Type> types = AvrPivotGridHelper.GetSearchFieldDataTypes(table);
            
            Assert.AreEqual(table.Rows.Count*4, types.Count); 
            Assert.AreEqual(typeof (bool), types["fBit"]);
            Assert.AreEqual(typeof (DateTime), types["fDate"]);
            Assert.AreEqual(typeof (float), types["fFloat"]);
            Assert.AreEqual(typeof (long), types["fID"]);
            Assert.AreEqual(typeof (long), types["fInteger"]);
            Assert.AreEqual(typeof (string), types["fString"]);
        }

        [TestMethod]
        public void QueryParamImageTest()
        {
            DataTable table = CreateLookupWithFF();

            Dictionary<string, Type> types = AvrPivotGridHelper.GetSearchFieldDataTypes(table);

            Assert.AreEqual(typeof(long), types["fNumeric"]);
            Assert.AreEqual(typeof(bool), types["fBoolean"]);
            Assert.AreEqual(typeof(string), types["fCaption"]);
            Assert.AreEqual(typeof(DateTime), types["fDate"]);
            Assert.AreEqual(typeof(DateTime), types["fDateTime"]);
            Assert.AreEqual(typeof(long), types["fMonths"]);
            Assert.AreEqual(typeof(string), types["fString"]);
            Assert.AreEqual(typeof(long), types["fNumericNatural"]);
            Assert.AreEqual(typeof(long), types["fNumericPositive"]);
            Assert.AreEqual(typeof(long), types["fNumericInteger"]);
        }

        [TestMethod]
        public void ImageListTest()
        {
            Assert.IsNotNull(AvrFieldIcons.ImageList);
            Assert.AreEqual(18, AvrFieldIcons.ImageList.Images.Count);
        }

        private static DataTable CreateLookupWithFieldType()
        {
            DataTable table = QueryLookupHelper.GetQuerySearchFieldLookupTable();
            table.Clear();
            AddRow(table, "fBit", 10081001, DBNull.Value);
            AddRow(table, "fDate", 10081002, DBNull.Value);
            AddRow(table, "fFloat", 10081004, DBNull.Value);
            AddRow(table, "fID", 10081005, DBNull.Value);
            AddRow(table, "fInteger", 10081006, DBNull.Value);
            AddRow(table, "fString", 10081007, DBNull.Value);
            return table;
        }

        private static DataTable CreateLookupWithFF()
        {
            DataTable table = QueryLookupHelper.GetQuerySearchFieldLookupTable();
            table.Clear();
            AddRow(table, "fNumeric", 10081003, 10071007);
            AddRow(table, "fBoolean", 10081003, 10071025);
            AddRow(table, "fCaption", 10081003, 10071026);
            AddRow(table, "fDate", 10081003, 10071029);
            AddRow(table, "fDateTime", 10081003, 10071030);
            AddRow(table, "fMonths", 10081003, 10071037);
            AddRow(table, "fString", 10081003, 10071045);
            AddRow(table, "fNumericNatural", 10081003, 10071059);
            AddRow(table, "fNumericPositive", 10081003, 10071060);
            AddRow(table, "fNumericInteger", 10081003, 10071061);
            return table;
        }

        private static void AddRow(DataTable table, string alias, object fieldType, object paramType)
        {
            DataRow row = table.NewRow();

            m_Count++;
            row["idfQuerySearchField"] = m_Count;
            row["idflQuery"] = 1;

            row["FieldAlias"] = alias;
            row["idfsSearchFieldType"] = fieldType;
            if (paramType != DBNull.Value)
            {
                row["idfsParameterType"] = paramType;
            }
            table.Rows.Add(row);
        }
    }
}
