using System;
using Microsoft.ML;
using System.Linq;

namespace DroneSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MLContext();

            // Kreiraj model mašinskog učenja za predviđanje smjera i brzine
            var model = CreateMLModel(context);

            // Kreiraj drona
            var drone = new DroneControl1();

            // Simuliraj podatke sa senzora
            var sensorData = new FlightData { SensorInput1 = 0.4f, SensorInput2 = 0.3f };

            // Predvidi smjer i brzinu pomoću modela
            var prediction = PredictDirectionAndSpeed(model, sensorData);

            // Pomiči drona prema predviđenom smjeru i brzini
            drone.Move(prediction.PredictedDirection, prediction.PredictedSpeed);
            drone.ChangeAltitude(10); // Povećaj visinu
        }

        static ITransformer CreateMLModel(MLContext context)
        {
            // Definiraj podatke za treniranje (u stvarnosti bi podaci dolazili od senzora)
            var trainingData = new[]
            {
        new FlightData { SensorInput1 = 0.5f, SensorInput2 = 0.2f, Direction = 1f, Speed = 1f },
        new FlightData { SensorInput1 = 0.6f, SensorInput2 = 0.1f, Direction = 0.8f, Speed = 1.2f },
        new FlightData { SensorInput1 = 0.7f, SensorInput2 = 0.3f, Direction = 1.5f, Speed = 0.9f }
    };

            var trainData = context.Data.LoadFromEnumerable(trainingData);
            var pipeline = context.Regression.Trainers.FastTree();

            return pipeline.Fit(trainData);
        }

        static FlightPrediction PredictDirectionAndSpeed(ITransformer model, FlightData sensorData)
        {
            var context = new MLContext();
            var predictionFunction = model.MakePredictionFunction<FlightData, FlightPrediction>(context);
            var prediction = predictionFunction.Predict(sensorData);
            return prediction;
        }
    }
}