namespace Recommender
    module App=

    open Nancy
    open Recommender.Core
    type App() as this =
        inherit NancyModule()
        do
            let _recommender = RecommendationService()
            this.Get.["/"] <- fun _ -> "Hello World!" :> obj
            this.Get.["/Recommend/{userId}/{limit}"] <- fun parameters ->
                let userId = (parameters :?> Nancy.DynamicDictionary).["userId"] :?> int64
                let limit = (parameters :?> Nancy.DynamicDictionary).["limit"] :?> int
                this.Response.AsJson(_recommender.Recommend(userId, limit)) :> obj