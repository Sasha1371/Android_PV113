pipeline {
    agent any

    environment {
        DOCKER_CREDENTIALS_ID = '65c7ae28-1031-4eb9-93c3-3f058882703b'
        SQL_CONTAINER_NAME = 'lendy123/postgres'
        FRONTEND_CONTAINER_NAME = 'lendy123/pizza-app'
        BACKEND_CONTAINER_NAME = 'lendy123/mypizza-client'
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
        stage('Тегування postgres зображення') {
            steps {
                script {
                    sh 'docker tag postgres lendy123:version${BUILD_NUMBER}'
                }
            }
        }
        stage('Пуш postgres в Docker Hub') {
            steps {
                script {
                    sh 'docker push lendy123/postgres:version${BUILD_NUMBER}'
                }
            }
        }
        stage('Тегування pizza-app зображення') {
            steps {
                script {
                    sh 'docker tag pizza-app lendy123:version${BUILD_NUMBER}'
                }
            }
        }
        stage('Пуш pizza-app в Docker Hub') {
            steps {
                script {
                    sh 'docker push lendy123/pizza-app:version${BUILD_NUMBER}'
                }
            }
        }
        stage('Тегування client-container зображення') {
            steps {
                script {
                    sh 'docker tag mypizza-client lendy123/mypizza-client:version${BUILD_NUMBER}'
                }
            }
        }
        stage('Пуш client-container в Docker Hub') {
            steps {
                script {
                    sh 'docker push lendy123/mypizza-client:version${BUILD_NUMBER}'
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
