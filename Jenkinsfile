pipeline {
    agent any

    environment {
        DOCKER_CREDENTIALS_ID = 'd0777b7b-6d7f-47d7-96db-da18b6772fe0'
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
                    sh 'docker tag postgres lendy123/postgres:version${BUILD_NUMBER}'
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
                    sh 'docker tag pizza-app lendy123/pizza-app:version${BUILD_NUMBER}'
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
        stage('Тегування mypizza-client зображення') {
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
        stage('Чистка старих образів') {
            steps {
                script {
                    sh 'docker image prune -a --filter "until=24h" --force'
                }
            }
        }
    }
}
