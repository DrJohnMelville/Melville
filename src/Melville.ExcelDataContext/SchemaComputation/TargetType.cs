using System;

namespace Melville.ExcelDataContext.SchemaComputation
{
    public class TargetType
    {
        public ISchemaTargetType? FinalSchema => 
            hasNoInvalidData && hasValidData ? ComputeSchema(): null;

        private ISchemaTargetType ComputeSchema() => 
            hasNull ?  new UnknownSchemaAdaptor(baseSchemaComputer): baseSchemaComputer;

        private readonly ISchemaTargetType baseSchemaComputer;
        private Func<string, bool> test;
        public bool hasNoInvalidData { get; set; } = true;
        private bool hasNull = false;
        private bool hasValidData = false;

        public TargetType(Func<string, bool> test, SchemaTargetType baseSchemaComputer)
        {
            this.test = test;
            this.baseSchemaComputer = baseSchemaComputer;
        }

        public void DoTest(string sample)
        {
            if (!hasNoInvalidData) return;
            if (DoNullCheck(sample)) return;
            CatagorizeSample(sample);
        }

        private void CatagorizeSample(string sample)
        {
            if (!test(sample))
            {
                hasNoInvalidData = false;
            }
            else
            {
                hasValidData = true;
            }
        }

        private bool DoNullCheck(string sample)
        {
            if (string.IsNullOrWhiteSpace(sample))
            {
                hasNull = true;
                return true;
            }
            return false;
        }
    }
}