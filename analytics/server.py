from typing import Optional, List, Any, Dict, Generic, TypeVar, Type

from bson import ObjectId
from fastapi import FastAPI, Depends
from fastapi.encoders import jsonable_encoder
from motor.motor_asyncio import AsyncIOMotorClient
from pydantic import BaseSettings, validator, BaseModel, Field


ChoiceType = TypeVar("ChoiceType")
ModelType = TypeVar("ModelType", bound=BaseModel)

Card = str
Relic = str


class _Settings(BaseSettings):
    MONGO_HOST: str
    MONGO_PORT: int
    # MONGO_USER: str
    # MONGO_PASS: str
    MONGO_DB: str
    MONGO_URL: Optional[str] = None

    @validator("MONGO_URL")
    def assemble_mongo_connection(cls, v, values):
        if isinstance(v, str):
            return v
        # return f"mongodb://{values.get('MONGO_USER')}:{values.get('MONGO_PASS')}@{values.get('MONGO_HOST')}:{values.get('MONGO_PORT')}/{values.get('MONGO_DB')}"
        return f"mongodb://{values.get('MONGO_HOST')}:{values.get('MONGO_PORT')}/{values.get('MONGO_DB')}"

    class Config:
        case_sensitive = True


settings = _Settings()


class DataBase:
    client: AsyncIOMotorClient = None


db = DataBase()


async def get_db() -> AsyncIOMotorClient:
    return db.client


async def connect_to_mongo():
    db.client = AsyncIOMotorClient(settings.MONGO_URL)[settings.MONGO_DB]


async def close_mongo_connection():
    db.client.close()


class PyObjectId(ObjectId):
    # https://www.mongodb.com/developer/quickstart/python-quickstart-fastapi/#the-_id-attribute-and-objectids
    @classmethod
    def __get_validators__(cls):
        yield cls.validate

    @classmethod
    def validate(cls, v):
        if not ObjectId.is_valid(v):
            raise ValueError("Invalid objectid")
        return ObjectId(v)

    @classmethod
    def __modify_schema__(cls, field_schema):
        field_schema.update(type="string")

    class Config:
        allow_population_by_field_name = True
        arbitrary_types_allowed = True
        json_encoders = {ObjectId: str}


class BaseChoices(BaseModel, Generic[ChoiceType]):
    chosen: ChoiceType
    not_chosen: List[ChoiceType]


class CardChoices(BaseChoices[Card]):
    pass


class _Model(BaseModel):
    id: PyObjectId = Field(default_factory=PyObjectId, alias="_id")

    class Config:
        allow_population_by_field_name = True
        json_encoders = {ObjectId: str}
        arbitrary_types_allowed = True


class RunModel(_Model):
    rng_seed: int
    game_version: str
    chosen_path: List[str]
    normal_deck: List[str]
    special_deck: List[Dict]
    card_choices: List[CardChoices]
    relics: List[Relic]

    class Config:
        allow_population_by_field_name = True
        json_encoders = {ObjectId: str}


class RunPostSchema(BaseModel):
    # Having basically identical schemas and models is actually the correct way to do this
    rng_seed: int
    game_version: str
    chosen_path: List[str]
    normal_deck: List[str]
    special_deck: List[Dict]
    card_choices: List[CardChoices]
    relics: List[Relic]


class _CRUDBase(Generic[ModelType]):
    def __init__(self, collection: str, model: Type[ModelType]):
        self.collection = collection
        self.model = model

    async def create(self, db: AsyncIOMotorClient, *, obj_in: ModelType) -> Dict:
        obj_in_data = jsonable_encoder(obj_in)
        new_obj = await db[self.collection].insert_one(obj_in_data)
        return await db[self.collection].find_one({"_id": new_obj.inserted_id})

    async def get(self, db: AsyncIOMotorClient, *, id_: int) -> Optional[Dict]:
        return await db[self.collection].find_one({"_id": id_})


class CRUDRun(_CRUDBase[RunModel]):
    pass


crud_run = CRUDRun("runs", RunModel)


app = FastAPI()


@app.post("/v1/run", response_model=RunModel)
async def post_run(obj_in: RunPostSchema, db=Depends(get_db)):
    x = await crud_run.create(db, obj_in=obj_in)
    print(x)
    return x


app.add_event_handler("startup", connect_to_mongo)
app.add_event_handler("shutdown", close_mongo_connection)
