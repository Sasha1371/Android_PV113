pipeline {
    agent any

    environment {
        DOCKER_CREDENTIALS_ID = 'bb9a94fe-56df-49db-a591-e63fd2662a95'
        SQL_CONTAINER_NAME = 'lendy123/api-postgresql-container'
        FRONTEND_CONTAINER_NAME = 'lendy123/client-container'
        BACKEND_CONTAINER_NAME = 'lendy123/api-pizza-container'
    }
    stages {
        stage('Вхід у DockerHub') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: DOCKER_CREDENTIALS_ID, passwordVariable: 'DOCKER_PASSWORD', usernameVariable: 'DOCKER_USERNAME')]) {
                        sh 'echo $DOCKER_PASSWORD | docker login --username $DOCKER_USERNAME --password-stdin'
                    }
                }
            }
        }
        stage('Білд проекту') {
            steps {
                script {
                     dir('Android_PV113') {
                        sh 'docker-compose up -d'
                     }
                }
            }
        }
        stage('Тегування api-postgresql-container зображення') {
            steps {
                script {
                    sh 'docker tag lendy123/api-postgresql-container:version${BUILD_NUMBER} lendy123/api-postgresql-container:latest'
                }
            }
        }
        stage('Пуш api-postgresql-container в Docker Hub') {
            steps {
                script {
                    sh 'docker push lendy123/api-postgresql-container:version${BUILD_NUMBER}'
                    sh 'docker push lendy123/api-postgresql-container:latest'
                }
            }
        }
        stage('Тегування api-pizza-container зображення') {
            steps {
                script {
                    sh 'docker tag lendy123/api-pizza-container:version${BUILD_NUMBER} lendy123/api-pizza-container:latest'
                }
            }
        }
        stage('Пуш api-pizza-container в Docker Hub') {
            steps {
                script {
                    sh 'docker push lendy123/api-pizza-container:version${BUILD_NUMBER}'
                    sh 'docker push lendy123/api-pizza-container:latest'
                }
            }
        }
        stage('Тегування client-container зображення') {
            steps {
                script {
                    sh 'docker tag lendy123/client-container:version${BUILD_NUMBER} lendy123/client-container:latest'
                }
            }
        }
        stage('Пуш client-container в Docker Hub') {
            steps {
                script {
                    sh 'docker push lendy123/client-container:version${BUILD_NUMBER}'
                    sh 'docker push lendy123/client-container:latest'
                }
            }
        }
        stage('Зупинка та видалення старого контейнера FrontEnd') {
            steps {
                script {
                    sh """
                    if [ \$(docker ps -aq -f name=${FRONTEND_CONTAINER_NAME}) ]; then
                        docker stop ${FRONTEND_CONTAINER_NAME}
                        docker rm ${FRONTEND_CONTAINER_NAME}
                    else
                        echo "Контейнер ${FRONTEND_CONTAINER_NAME} не знайдено. Продовжуємо..."
                    fi
                    """
                }
            }
        }
        stage('Зупинка та видалення старого контейнера BackEnd') {
            steps {
                script {
                    sh """
                    if [ \$(docker ps -aq -f name=${BACKEND_CONTAINER_NAME}) ]; then
                        docker stop ${BACKEND_CONTAINER_NAME}
                        docker rm ${BACKEND_CONTAINER_NAME}
                    else
                        echo "Контейнер ${BACKЕНD_CONTAINER_NAME} не знайдено. Продовжуємо..."
                    fi
                    """
                }
            }
        }
        stage('Зупинка та видалення старого контейнера MySQL') {
            steps {
                script {
                    sh """
                    if [ \$(docker ps -aq -f name=${SQL_CONTAINER_NAME}) ]; then
                        docker stop ${SQL_CONTAINER_NAME}
                        docker rm ${SQL_CONTAINER_NAME}
                    else
                        echo "Контейнер ${SQL_CONTAINER_NAME} не знайдено. Продовжуємо..."
                    fi
                    """
                }
            }
        }    
        stage('Чистка старих образів') {
            steps {
                script {
                    sh 'docker image prune -a --filter "until=24h" --force'
                }
            }
        }
    }
}
