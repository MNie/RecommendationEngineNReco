namespace Recommender.Core
    open FSharp.Data.TypeProviders
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
                    for row in db.Recommender_DataModelView_New do
                    select row
                }
                |> Seq.groupBy (fun row -> row.User_id)
            let data = new FastByIDMap<FastIDSet>(dataFromDb |> Seq.length)
            dataFromDb
            |> Seq.iter (fun (key, values) ->
                    let value = values |> Seq.map (fun x -> x.Item_id) |> Seq.toArray |> (fun x -> FastIDSet(x))
                    data.Put(key, value) |> ignore
                )
            GenericBooleanPrefDataModel(data)