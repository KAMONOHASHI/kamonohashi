using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Logic.Interfaces;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    public class DataSetLogic : PlatypusLogicBase, IDataSetLogic
    {
        private IDataSetRepository DataSetRepository { get; }
        private IInferenceHistoryRepository InferenceHistoryRepository { get; }
        private ITrainingHistoryRepository TrainingHistoryRepository { get; }
        private IAquariumDataSetVersionRepository AquariumDataSetVersionRepository { get; }


        public DataSetLogic(IDataSetRepository dataSetRepository,
            IInferenceHistoryRepository inferenceHistoryRepository,
            ITrainingHistoryRepository trainingHistoryRepository,
            IAquariumDataSetVersionRepository aquariumDataSetVersionRepository,
            ICommonDiLogic commonDiLogic) 
        : base(commonDiLogic)
        {
            DataSetRepository = dataSetRepository;
            InferenceHistoryRepository = inferenceHistoryRepository;
            TrainingHistoryRepository = trainingHistoryRepository;
            AquariumDataSetVersionRepository = aquariumDataSetVersionRepository;
        }

        public async Task<bool> ReleaseLockAsync(long dataSetId)
        {
            if (await InferenceHistoryRepository.Count(x => x.DataSetId == dataSetId)
                + await TrainingHistoryRepository.Count(x => x.DataSetId == dataSetId)
                + await AquariumDataSetVersionRepository.Count(x => x.DataSetId == dataSetId)
                <= 1)
            {
                var dataSet = await DataSetRepository.GetByIdAsync(dataSetId);
                dataSet.IsLocked = false;
                return true;
            }
            return false;
        }
    }
}
