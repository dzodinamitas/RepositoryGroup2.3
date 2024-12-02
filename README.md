using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using System.Linq;

// Definicija podataka za treniranje
public class FlightData
{
    public float SensorInput1 { get; set; } // Prvi senzor, npr. udaljenost od prepreke
    public float SensorInput2 { get; set; } // Drugi senzor
    public float Direction { get; set; }    // Smjer kretanja
    public float Speed { get; set; }        // Brzina
}

public class FlightPrediction
{
    public float PredictedDirection { get; set; }
    public float PredictedSpeed { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Kreiranje ML.NET okruženja
        var context = new MLContext();

        // Pretpostavimo da imamo podatke za treniranje
        var trainingData = new[]
        {
            new FlightData { SensorInput1 = 0.5f, SensorInput2 = 0.2f, Direction = 1f, Speed = 1f },
            new FlightData { SensorInput1 = 0.6f, SensorInput2 = 0.1f, Direction = 0.8f, Speed = 1.2f },
            new FlightData { SensorInput1 = 0.7f, SensorInput2 = 0.3f, Direction = 1.5f, Speed = 0.9f },
            new FlightData { SensorInput1 = 0.2f, SensorInput2 = 0.5f, Direction = 1.2f, Speed = 1.1f }
        };

        var trainData = context.Data.LoadFromEnumerable(trainingData);

        // Definicija modela - treniranje neuronske mreže
        var pipeline = context.Regression.Trainers.FastTree();

        var model = pipeline.Fit(trainData);

        // Predviđanje smjera i brzine na temelju novih podataka
        var testData = new FlightData { SensorInput1 = 0.4f, SensorInput2 = 0.3f };
        var prediction = model.Transform(context.Data.LoadFromEnumerable(new[] { testData }));

        var predictions = context.Data.CreateEnumerable<FlightPrediction>(prediction, reuseRowObject: false).ToList();

        foreach (var pred in predictions)
        {
            Console.WriteLine($"Predicted Direction: {pred.PredictedDirection}, Predicted Speed: {pred.PredictedSpeed}");
        }
    }
}
class DroneControl
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float Rotation { get; set; } // Smjer rotacije

    // Funkcija za pomicanje drona u određenom smjeru
    public void Move(float direction, float speed)
    {
        // Smjer je od -1 do 1 (lijevo-desno, gore-dolje)
        X += direction * speed;
        Y += direction * speed;
        Console.WriteLine($"Drone moved to position X: {X}, Y: {Y}, Z: {Z}, Rotation: {Rotation}");
    }

    // Funkcija za rotiranje drona
    public void Rotate(float angle)
    {
        Rotation += angle;
        Console.WriteLine($"Drone rotated to angle: {Rotation}");
    }

    // Funkcija za kontroliranje visine drona
    public void ChangeAltitude(float altitude)
    {
        Z += altitude;
        Console.WriteLine($"Drone altitude changed to: {Z}");
    }
}
class Program
{
    static void Main(string[] args)
    {
        var context = new MLContext();

        // Kreiraj model mašinskog učenja za predviđanje smjera i brzine
        var model = CreateMLModel(context);

        // Kreiraj drona
        var drone = new DroneControl();

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
