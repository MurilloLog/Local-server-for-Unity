import  {prop, getModelForClass, Severity, mongoose} from '@typegoose/typegoose'

class Message {
    @prop({required: true, trim: true})
    _id: "string"
    
    @prop()
    objectAttribute1: "string"

    @prop()
    objectAttribute2: "Boolean"

    @prop({ allowMixed: Severity.ALLOW, type: () => mongoose.Schema.Types.Mixed })
    objectAttribute3:
    {
        x: "Decimal128",
        y: "Decimal128",
        z: "Decimal128"
    }
}

const MessageSchema = getModelForClass(Message)
export default MessageSchema