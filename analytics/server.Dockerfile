FROM tiangolo/uvicorn-gunicorn-fastapi:python3.8

# Install Poetry
RUN curl -sSL https://raw.githubusercontent.com/python-poetry/poetry/master/get-poetry.py | POETRY_HOME=/opt/poetry python && \
    cd /usr/local/bin && \
    ln -s /opt/poetry/bin/poetry && \
    poetry config virtualenvs.create false

COPY ./pyproject.toml ./poetry.lock* /app/

# Allow installing dev dependencies to run tests
ARG INSTALL_DEV=false
RUN bash -c "if [ $IS_PRODUCTION == 'true' ] ; then poetry install --no-root --no-dev ; else poetry install --no-root ; fi"

COPY server.py .