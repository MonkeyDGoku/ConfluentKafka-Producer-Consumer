## Simple example of Producer-Consumer in Kafka :smiley:

This project is created to understand the Prodeucer-Consumer model in Kafka, using Confluent Kafka in .NetCore
:exclamation:

### Dependecies

    1. Docker
    2. Conduktor
    3. Confluent kafka

### Essential Readings :blue_book:

1. [Confluent Kafka](https://docs.confluent.io/clients-confluent-kafka-dotnet/current/overview.html)

### Steps :page_with_curl:

1.  Install docker
2.  Pull docker images

```cmd
docker pull confluentinc/cp-zookeeper
docker pull confluentinc/cp-kafka
```

3.  Run servers

```cmd
docker network create kafka

docker run -d --network=kafka --name=zookeeper -e ZOOKEEPER_CLIENT_PORT=2181 -e ZOOKEPER_TICK_TIME=2000 -p 2181:2181 confluentinc/cp-zookeeper

docker run -d --network=kafka --name=kafka -p 9092:9092 -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 confluentinc/cp-kafka
```

4.  Install Conduktor to see the Topics
5.  Run the code
6.  Find the Topics, Consumer in Conduktor
