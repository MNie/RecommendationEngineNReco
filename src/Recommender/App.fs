namespace Recommender
    module App=

    open Nancy
    open Recommender.Core
    type App() as this =
        inherit NancyModule()
        let _recommender = RecommendationService()
        let (?) (parameters: obj) param =
                (parameters :?> Nancy.DynamicDictionary).[param].ToString()
        do
            this.Get.["/Recommend/{userId}/{limit}"] <- fun parameters ->
                System.Console.WriteLine(sprintf "%A, %A" parameters?userId parameters?limit)
                let userId = parameters?userId |> int64
                let limit = parameters?limit |> int
                this.Response.AsJson(_recommender.Recommend(userId, limit)) :> obj