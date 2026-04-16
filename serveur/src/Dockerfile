FROM python:trixie

COPY ./ ./

RUN wget http://security.ubuntu.com/ubuntu/pool/main/o/openldap/libldap-2.5-0_2.5.16+dfsg-0ubuntu0.22.04.2_amd64.deb -O ./lib.deb
RUN apt install ./lib.deb
RUN python3 -m pip install -r ./requirements.txt

CMD ["python3","./src/main.py"]