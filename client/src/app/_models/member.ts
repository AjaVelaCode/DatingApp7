import { Photo } from "./photo";

    export interface Member {
        id: number;
        userName: string;
        photoUrl: string;
        age: number;
        introduction: string;
        knownAs: string;
        created: Date;
        lastActive: Date;
        gender: string;
        lookingFor: string;
        interests: string;
        city: string;
        country: string;
        photos: Photo[];
    }


