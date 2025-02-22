import torch
import torch.onnx as onnx
import torch.nn as nn
import torch.optim as optim

class Network(nn.Module):
    def __init__(self):
        super().__init__()
        self.network = nn.Sequential(
            nn.Conv2d(1, 32, 3),
            nn.MaxPooling(2, 2),
            nn.Conv2d(32, 64, 3),
            nn.Linear(64, 128)
        )
        def forward(self, x):
            return self.network(x)
def main(epochs):
    model = Network()
    optimizer = optim.Adam(model.parameters())
    criterion = nn.CrossEntropyLoss()
    for epoch in range(epochs):
        optimizer.zero_grad()
        loss = criterion(model_image, target)
        loss.backward()
        optimizer.step()
        