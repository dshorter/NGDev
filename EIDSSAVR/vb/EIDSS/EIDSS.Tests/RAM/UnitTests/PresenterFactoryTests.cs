using System;
using bv.common.win;
using EIDSS.RAM;
using EIDSS.RAM.Presenters;
using EIDSS.RAM.Presenters.Base;
using EIDSS.RAM.Presenters.RamPivotGrid;
using EIDSS.RAM_DB.Views;
using EIDSS.Tests.RAM.UnitTests.Presenters;
using NMock2;
using NUnit.Framework;
using Is=NMock2.Is;

namespace EIDSS.Tests.RAM.UnitTests
{
    [TestFixture]
    public class PresenterFactoryTests
    {
        [SetUp]
        public void SetUp()
        {
            PresenterFactory.Init(new BaseForm());
        }

        [TearDown]
        public void TearDown()
        {
            GC.Collect();
        }

        [Test]
        public void CreateMapPresenterTest()
        {
            Mockery mocks = new Mockery();
            IMapView mapView = mocks.NewMock<IMapView>();
            Expect.Once.On(mapView).EventAdd("SendCommand", Is.Anything);
            Expect.Once.On(mapView).SetProperty("DBService");
            Expect.Once.On(mapView).EventAdd("InitAdmUnit", Is.Anything);
            Expect.Once.On(mapView).EventAdd("RefreshMap", Is.Anything);

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[mapView];
            Console.WriteLine(ramPresenter);
            Assert.IsTrue(ramPresenter is MapPresenter);

            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateChartPresenterTest()
        {
            Mockery mocks = new Mockery();
            IChartView chartView = mocks.NewMock<IChartView>();
            Expect.Once.On(chartView).EventAdd("SendCommand", Is.Anything);
            Expect.Once.On(chartView).EventAdd("ChangeOrientation", Is.Anything);
            Expect.Once.On(chartView).SetProperty("DBService");

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[chartView];
            Console.WriteLine(ramPresenter);
            Assert.IsTrue(ramPresenter is ChartPresenter);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreatePivotPresenterTest()
        {
            Mockery mocks = new Mockery();
            IPivotView pivotView = PivotPresenterTests.GetPivotView(mocks);
            //Expect.Once.On(pivotView).EventAdd("SendCommand", Is.Anything);
            //  Expect.Once.On(pivotView).SetProperty("DBService");

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[pivotView];
            Console.WriteLine(ramPresenter);
            Assert.IsTrue(ramPresenter is PivotPresenter);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateLayoutInfoPresenterTest()
        {
            Mockery mocks = new Mockery();
            ILayoutInfoView layoutInfoView = mocks.NewMock<ILayoutInfoView>();
            Expect.Once.On(layoutInfoView).EventAdd("SendCommand", Is.Anything);
            Expect.Once.On(layoutInfoView).EventAdd("LayoutSelecting", Is.Anything);
            Expect.Once.On(layoutInfoView).EventAdd("LayoutSelected", Is.Anything);
            Expect.Once.On(layoutInfoView).SetProperty("DBService");

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[layoutInfoView];
            Console.WriteLine(ramPresenter);
            Assert.IsTrue(ramPresenter is LayoutInfoPresenter);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateLayoutDetailPresenterTest()
        {
            Mockery mocks = new Mockery();
            ILayoutDetailView layoutDetailView = mocks.NewMock<ILayoutDetailView>();
            Expect.Once.On(layoutDetailView).EventAdd("SendCommand", Is.Anything);
            Expect.Once.On(layoutDetailView).SetProperty("DBService");
            Expect.Once.On(layoutDetailView).EventAdd("CopyLayoutCreating", Is.Anything);

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[layoutDetailView];
            Console.WriteLine(ramPresenter);
            LayoutDetailPresenter detailPresenter = ramPresenter as LayoutDetailPresenter;
            Assert.IsNotNull(detailPresenter);
            Assert.IsNotNull(detailPresenter.LayoutDbService);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateQueryInfoPresenterTest()
        {
            Mockery mocks = new Mockery();
            IQueryInfoView queryInfoView = mocks.NewMock<IQueryInfoView>();
            Expect.Once.On(queryInfoView).EventAdd("SendCommand", Is.Anything);
            Expect.Once.On(queryInfoView).SetProperty("DBService");

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[queryInfoView];
            Console.WriteLine(ramPresenter);

            QueryInfoPresenter infoPresenter = (ramPresenter as QueryInfoPresenter);
            Assert.IsNotNull(infoPresenter);

            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateRamPivotGridPresenterTest()
        {
            Mockery mocks = new Mockery();
            IRamPivotGridView pivotGridView = mocks.NewMock<IRamPivotGridView>();
            Expect.Once.On(pivotGridView).EventAdd("SendCommand", Is.Anything);
            Expect.Once.On(pivotGridView).EventAdd("FieldVisibleChanged", Is.Anything);
            //   Expect.Once.On(pivotGridView).GetProperty("Fields");

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[pivotGridView];
            Console.WriteLine(ramPresenter);
            Assert.IsTrue(ramPresenter is RamPivotGridPresenter);

            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreatePivotReportPresenterTest()
        {
            Mockery mocks = new Mockery();
            IPivotReportView pivotReportView = mocks.NewMock<IPivotReportView>();
            Expect.Once.On(pivotReportView).EventAdd("SendCommand", Is.Anything);
            Expect.Once.On(pivotReportView).SetProperty("DBService");

            BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[pivotReportView];
            Console.WriteLine(ramPresenter);
            Assert.IsTrue(ramPresenter is PivotReportPresenter);

            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateBaseLayoutPresenterTest()
        {
            try
            {
                Mockery mocks = new Mockery();
                IView view = mocks.NewMock<IView>();
                Expect.Once.On(view).EventAdd("SendCommand", Is.Anything);

                BaseRamPresenter ramPresenter = PresenterFactory.SharedPresenter[view];
                Assert.IsNotNull(ramPresenter);
                mocks.VerifyAllExpectationsHaveBeenMet();
            }
            catch (NotSupportedException)
            {
                Console.WriteLine(@"Test ok");
            }
        }
    }
}