﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Data.Filtering;

namespace eidss.avr.mweb.Utils
{

    public class FilterValidator : IClientCriteriaVisitor<object>
    {
        public string Result { get; private set; }
        #region IClientCriteriaVisitor Members

        public object Visit(JoinOperand theOperand)
        {
            return theOperand;
        }

        public object Visit(OperandProperty theOperand)
        {
            return theOperand;
        }

        public object Visit(AggregateOperand theOperand)
        {
            return theOperand;
        }

        #endregion

        #region ICriteriaVisitor Members

        public object Visit(FunctionOperator theOperator)
        {
            if (theOperator.Operands.Count == 0)
                return theOperator;
            var prop = theOperator.Operands[0] as OperandProperty;
            if (ReferenceEquals(prop,null))
                return theOperator;
            //Replace supported outlook intervals by explict dates because web prefilter doesn't support 
            switch (theOperator.OperatorType)
            {
                case FunctionOperatorType.IsOutlookIntervalToday:
                    return CreateDateRangeCriteria(prop.PropertyName, DateTime.Today,
                                                   DateTime.Today.AddDays(1));
                case FunctionOperatorType.IsOutlookIntervalYesterday:
                    return CreateDateRangeCriteria(prop.PropertyName, DateTime.Today.AddDays(-1),
                                                   DateTime.Today);
                case FunctionOperatorType.IsOutlookIntervalEarlierThisWeek:
                    return CreateDateRangeCriteria(prop.PropertyName, DateTime.Today.AddDays(1-(int)DateTime.Today.DayOfWeek),
                                                   DateTime.Today);
                case FunctionOperatorType.IsOutlookIntervalLastWeek:
                    return CreateDateRangeCriteria(prop.PropertyName, DateTime.Today.AddDays(6-(int)DateTime.Today.DayOfWeek),
                                                   DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek));
                case FunctionOperatorType.IsOutlookIntervalEarlierThisMonth:
                    return CreateDateRangeCriteria(prop.PropertyName, DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek),
                                                   new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                case FunctionOperatorType.IsOutlookIntervalEarlierThisYear:
                    return CreateDateRangeCriteria(prop.PropertyName, new DateTime(DateTime.Now.Year, 1, 1),
                                                   new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                case FunctionOperatorType.IsOutlookIntervalPriorThisYear:
                    return new BinaryOperator(prop.PropertyName, new DateTime(DateTime.Now.Year, 1, 1),
                                              BinaryOperatorType.Less);
            }
            foreach (var operand in theOperator.Operands)
            {
                operand.Accept(this);
            }
            return theOperator;
        }

        public object Visit(OperandValue theOperand)
        {
            return theOperand;
        }

        public object Visit(GroupOperator theOperator)
        {
            foreach (var operand in theOperator.Operands)
            {
                operand.Accept(this);
            }
            return theOperator;
        }

        public object Visit(InOperator theOperator)
        {
            return null;
        }

        public object Visit(UnaryOperator theOperator)
        {
            return theOperator;
        }

        public object Visit(BinaryOperator theOperator)
        {
            theOperator.LeftOperand.Accept(this);
            theOperator.RightOperand.Accept(this);
            return theOperator;
        }

        public object Visit(BetweenOperator theOperator)
        {
            return theOperator;
        }

        #endregion

        CriteriaOperator CreateDateRangeCriteria(string propertyName, DateTime? startDate, DateTime? endDate)
        {
            var groupOp = new GroupOperator();
            if (startDate != null)
            {
                groupOp.Operands.Add(new BinaryOperator(propertyName, startDate.Value, BinaryOperatorType.GreaterOrEqual));
            }
            if (endDate != null)
            {
                groupOp.Operands.Add(new BinaryOperator(propertyName, endDate.Value, BinaryOperatorType.Less));
            }
            return groupOp;

        }
    }

}