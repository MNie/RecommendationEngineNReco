namespace Recommender.Core

    open FSharp.Data.TypeProviders
    open Newtonsoft.Json
    open NReco.CF.Taste.Model
    open NReco.CF.Taste.Common
    open NReco.CF.Taste.Impl.Common
    open NReco.CF.Taste.Impl.Model

    type dbSchema = SqlDataConnection<ConnectionStringName = "db", ConfigFile = "App.config">

    type DataProvider() =
        member this.GetModelData() =
            let db = dbSchema.GetDataContext()
            let dataFromDb =
                query {
                    for row in db.DataModelView do
                    select row
                }
                |> Seq.groupBy (fun row -> row.user_id)
            let data = new FastByIDMap<FastIDSet>(dataFromDb |> Seq.length)
            dataFromDb
            |> Seq.iter (fun x -> data.Put(x.Key, new FastIDSet(x.Values)))
            GenericBooleanPrefDataModel(data)