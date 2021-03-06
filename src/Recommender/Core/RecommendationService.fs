namespace Recommender.Core

open NReco.CF.Taste.Model
open NReco.CF.Taste.Common
open NReco.CF.Taste.Impl.Common
open NReco.CF.Taste.Impl.Recommender
open NReco.CF.Taste.Impl.Similarity
open NReco.CF.Taste.Impl.Neighborhood
open System.Configuration
open FSharp.Configuration

type Settings = AppSettings<"App.config">

type RecommendationService() =
    let mutable _recommender = null
    let setRecommender() =
        _recommender <-
            let model = (DataProvider()).GetModelData()
            let similarity = LogLikelihoodSimilarity(model)
            let neighborhood =
                (Settings.NeighborhoodLimit, Settings.NeighborhoodTreshold, similarity, model)
                |> NearestNUserNeighborhood
                |> fun x -> CachingUserNeighborhood(x, model)
            GenericBooleanPrefUserBasedRecommender(model, neighborhood, similarity)
    do setRecommender()

    member this.Recommend(userId, limit) =
        _recommender.Recommend(userId, limit)