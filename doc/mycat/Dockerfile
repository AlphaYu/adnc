FROM openjdk:8-jdk-stretch

ADD http://dl.mycat.org.cn/1.6.7.6/20201126013625/Mycat-server-1.6.7.6-release-20201126013625-linux.tar.gz /usr/local
RUN cd /usr/local && tar -zxvf Mycat-server-1.6.7.6-release-20201126013625-linux.tar.gz && ls -lna

ENV MYCAT_HOME=/usr/local/mycat
WORKDIR /usr/local/mycat

ENV TZ Asia/Shanghai

EXPOSE 8066 9066

CMD ["/usr/local/mycat/bin/mycat", "console","&"]