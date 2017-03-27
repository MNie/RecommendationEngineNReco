namespace Recommender.Core

open NReco.CF.Taste.Model
open NReco.CF.Taste.Common
open NReco.CF.Taste.Impl.Common
open NReco.CF.Taste.Impl.Recommender
open NReco.CF.Taste.Impl.Similarity
open NReco.CF.Taste.Impl.Neighborhood
type RecommendationService() =
    let _recommender =
        let model = (DataProvider()).GetModelData()
        let similarity = LogLikelihoodSimilarity(model)
        let neighborhood = CachingUserNeighborhood(NearestNUserNeighborhood(100, 0.1, similarity, model), model)
        GenericBooleanPrefUserBasedRecommender(model, neighborhood, similarity)

    member this.Recommend(userId, limit) =
        _recommender.Recommend(userId, limit)