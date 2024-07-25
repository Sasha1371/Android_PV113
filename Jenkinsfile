pipeline {
    agent any

    stages {
        stage('Білд проекту') {
            steps {
                script {
                    dir('Android_PV113') {
                        sh 'docker-compose up -d'
                    }
                }
            }
        }
    }
}
