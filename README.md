# TotalAgilityUnitel
Sistema de gestão de alguns serviços usados no KTA

# Language
1. C#

# Framework
1. .NET CORE 8.0

# Data Base
1. SqlServer

# Monitoring
1. Prometheus
2. Grafana

# Messageria
1. RabbitMQ
2. Run RabbitMQ: docker run -d --hostname localhost --name rabbitmq_unitel -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=Admin2k24@ rabbitmq:3-management
3. browser: http://localhost:15672/

# Containers
1. Docker
2. docker-compose
3. Requirements: Docker installed
4. Run container: docker-compose up -d
5. Down container: docker-compose down

