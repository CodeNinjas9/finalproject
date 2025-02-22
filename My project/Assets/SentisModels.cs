using Unity.Sentis;
public class SentisModels: MonoBehaviour(){
    public string onnxFilePath;
    public Model runtimeModel;
    void Start(){
        runtimeModel = ModelLoader.Load(onnxFilePath);
    }
}
