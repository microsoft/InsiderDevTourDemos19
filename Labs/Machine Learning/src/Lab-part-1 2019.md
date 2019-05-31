In this section, we present context for the lab, providing an overview of the intelligent edge and machine learning.

## Intelligent edge

The advent of intelligent cloud computing has created a pattern of sending data to cloud for processing data at scale, but recently, a new term has started to appear and gain traction: intelligence on the edge. Intelligence on the edge, or edge computing, refers to processing data as close to the source as possible and allowing systems to perform operational decisions directly, usually with the help of machine learning models. Instead of sending data to the cloud and waiting for feedback, the whole process is performed close to the data (on the edge).

By processing data at the edge instead of forcing the data to complete a round-trip to remote servers, there are a number of immediate technical advantages: reduced latency, improved quality of data (no bandwidth concerns), and privacy of data. Even better, the technical advantages enable intelligent edge scenarios, such as self-driving cars. Detecting a failing machine in an industrial plant based on noise and vibration, and shutting down the assembly line to prevent further issues is also a common improvement in many industries. These scenarios are possible because they use intelligence on the edge, or the ability to process the data as close to the source as possible.

Even though these scenarios are tied to the intelligent edge, the basis of the data analysis and processing has its roots in an established field: Artificial Intelligence (AI) and Machine Learning (ML). Let's take a step back, and get to know ML a bit better.

## Machine learning

### What is ML? 

Machine learning is a term coined in 1959 by Arthur Samuel and currently defines a field of computer science. By leveraging statistical techniques, machine learning gives machines the ability to learn, improving their performance on a specific task. This is done by combining ML algorithms and huge amounts of data. By processing previously collected data, ML algorithms generate models that can predict the correct output when presented with a new input.

This learning capability is especially useful in scenarios where designing and programming explicit algorithms with good performance is difficult or infeasible. The most typical scenarios include detecting spam emails, optical character recognition, computer vision or recommendation algorithms.

### What is a model?

A "machine learning model" is generated when a machine learning algorithm is trained with a training dataset.

For example, consider the scenario where we want to detect spam email, where the objective is to generate a model that takes in an input in real-time (an email), and generates an output (whether the email is considered spam or not). The training set would include emails labeled as spam or not spam, and the algorithm would use features or attributes of each email (contents, header, sender domain, etc.) to learn the typical attributes of a spam email.

This model-building phase is called "training." Once trained with existing data, the model can perform predictions with new, previously unseen data, which is called "inferencing," "evaluation," or "scoring."

There is a huge ecosystem of ML frameworks and tools for training and evaluting models both in the cloud and on the edge, and in this lab, you'll learn how to use a few of Microsoft's ML services and APIs.

### What is ONNX?

Open Neural Network Exchange (ONNX) provides an open format for ML models, allowing you to import and export to numerous ML frameworks. ONNX defines an extensible computation graph model, as well as definitions of built-in operators and standard data types. ONNX is being co-developed by Microsoft, Amazon and Facebook as an open-source project. In its initial release, the project supports Caffe2, PyTorch, MXNet and Microsoft CNTK Deep learning framework.

In this lab, you'll learn how to evaluate ONNX models with Windows ML, applying intelligence on the edge.